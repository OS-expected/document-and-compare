using docAndCom.Helpers;
using docAndCom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Plugin.Calendar.Models;
using static docAndCom.Helpers.ShortenInvokes;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentPage : ContentPage
    {
        public EventCollection Events { get; set; }

        public DocumentPage()
        {
            InitializeComponent();          
        }

        protected override void OnAppearing()
        {
            ConfigureCalendar();

            InitEventsInCalendar();
        }

        private async void ToolbarItem_Document_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewDocumentPage());
        }

        private void OpenImageBtn_Clicked(object sender, EventArgs e)
        {
            OperationsOnImages.Open(sender);
        }

        private void ConfigureCalendar()
        {
            calendarRef.Culture = ResourceLoader.Instance.GetCultureInfo();
            calendarRef.DaysTitleLabelStyle = new Style(typeof(Entry))
            {
                Setters = {
                new Setter { Property = Entry.FontSizeProperty, Value = 10 }
            }
            };
            calendarRef.SelectedDayBackgroundColor = Color.FromHex("#03396C");
            calendarRef.SelectedDayTextColor = Color.White;
        }

        private async void HardDelBtn_Clicked(object sender, EventArgs e)
        {
            string hardDelData = ((Button)sender).BindingContext as string;
            string[] arr = hardDelData.Split(new[] { '|' }, 2); // 0 -> path, 1 -> tag
  
            var hardDelConfirmationAlertDesc = GetResourceString("hardDeleteAlertText");
            hardDelConfirmationAlertDesc = hardDelConfirmationAlertDesc.Replace("<%tagName%>", arr[1]);

            bool answer = await DisplayAlert(GetResourceString("areYouSureText"),
                hardDelConfirmationAlertDesc, GetResourceString("YesText"),
                GetResourceString("NoText"));

            if(answer == true)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    var tagName = arr[1];
                    var tag = conn.Table<Tag>().FirstOrDefault(t => t.Name == tagName);

                    if (tag == null)
                    {
                        await DisplayAlert(GetResourceString("OopsText"), 
                            GetResourceString("imageTagRefNotFoundText"),
                            GetResourceString("OkText"));
                        return;
                    }

                    var photoPath = arr[0];
                    var photo = conn.Table<Photo>().FirstOrDefault(p => p.Path == photoPath && p.TagId == tag.Id);

                    if (photo == null)
                    {
                        await DisplayAlert(GetResourceString("OopsText"),
                            GetResourceString("imageRefNotFoundText"),
                            GetResourceString("OkText"));
                        return;
                    }

                    var res = conn.Delete<Photo>(photo.Id);

                    if (File.Exists(photo.Path))
                    {
                        File.Delete(photo.Path);
                    }

                    string extraNote = GetResourceString("imageLeftText");

                    if (File.Exists(photo.Path) == false)
                    {
                        extraNote = GetResourceString("imageRemovedText");
                    }

                    InitEventsInCalendar();

                    if (res > 0)
                    {
                        await DisplayAlert(GetResourceString("SuccessText"),
                            GetResourceString("referenceClearedText") + extraNote,
                            GetResourceString("OkText"));
                    } else if (res <= 0)
                    {
                        await DisplayAlert(GetResourceString("OopsText"),
                            GetResourceString("referenceNotClearedText") + extraNote,
                            GetResourceString("OkText"));
                    }
                }
            }
        }
        
        private void InitEventsInCalendar()
        {
            Events = new EventCollection();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Photo>();

                var photos = conn.Table<Photo>().ToList();

                if (photos.Count <= 0)
                {
                    return;
                }

                var days = (from table in conn.Table<Photo>()
                            select table.CreatedOn).Distinct().ToList();

                foreach (var day in days)
                {
                    List<EventModel> list = new List<EventModel>();

                    foreach (var photoObj in photos)
                    {
                        if (photoObj.CreatedOn == day)
                        {
                            var tagName = conn.Table<Tag>().SingleOrDefault(t => t.Id == photoObj.TagId).Name;

                            if (string.IsNullOrEmpty(tagName))
                            {
                                tagName = "undefined?";
                            }

                            EventModel eventModel = new EventModel()
                            {
                                Tag = tagName,
                                ImagePath = photoObj.Path,
                                ClumpedData = photoObj.Path + "|" + tagName
                            };

                            list.Add(eventModel);
                        }
                    }
                    Events.Add(day, list);
                }
            }

            calendarRef.Events = Events;
        }
    }
}