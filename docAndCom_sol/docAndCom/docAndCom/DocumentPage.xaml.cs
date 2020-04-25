﻿using docAndCom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Plugin.Calendar.Models;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentPage : ContentPage
    {
        public EventCollection Events { get; set; }

        public DocumentPage()
        {
            InitializeComponent();

            InitEventsInCalendar();
        }

        protected override void OnAppearing()
        {
            InitEventsInCalendar();
        }

        private async void ToolbarItem_Document_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewDocumentPage());
        }

        private void OpenImageBtn_Clicked(object sender, EventArgs e)
        {
            string path = ((Button)sender).BindingContext as string;

            DependencyService.Get<IFileOpener>().OpenFileByGivenPath(path);
        }

        private async void HardDelBtn_Clicked(object sender, EventArgs e)
        {
            string hardDelData = ((Button)sender).BindingContext as string;
            string[] arr = hardDelData.Split(new[] { '|' }, 2); // 0 -> path, 1 -> tag

            bool answer = await DisplayAlert("Are you sure?", $"This operation will permanently remove documented image(of {arr[1]} tag) from storage device.", "Yes", "No");

            if(answer == true)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    var tagName = arr[1];
                    var tag = conn.Table<Tag>().SingleOrDefault(t => t.Name == tagName);

                    if (tag == null)
                    {
                        await DisplayAlert("Oops..", "Image tag reference not found. Operation aborted.", "Ok");
                        return;
                    }

                    var photoPath = arr[0];
                    var photo = conn.Table<Photo>().SingleOrDefault(p => p.Path == photoPath && p.TagId == tag.Id);

                    if (photo == null)
                    {
                        await DisplayAlert("Failure", "Operation aborted. Reference to the image not found.", "Ok");
                        return;
                    }

                    var res = conn.Delete<Photo>(photo.Id);

                    if (File.Exists(photo.Path))
                    {
                        File.Delete(photo.Path);
                    }

                    string extraNote = "image left on device storage.";

                    if (File.Exists(photo.Path))
                    {
                        extraNote = "image removed from device storage.";
                    }

                    if (res > 0)
                    {
                        await DisplayAlert("Success", "Reference cleared, " + extraNote, "Ok");
                        InitEventsInCalendar();
                    } else if (res <= 0)
                    {
                        await DisplayAlert("Oops..", "Reference not cleared, " + extraNote, "Ok");
                    }
                }
            }
        }

        private async void ClearRefBtn_Clicked(object sender, EventArgs e)
        {
            string clearRefData = ((Button)sender).BindingContext as string;
            string[] arr = clearRefData.Split(new[] { '|' }, 2); // 0 -> path, 1 -> tag

            bool answer = await DisplayAlert("Are you sure?", $"This operation will remove reference to the documented image from {arr[1]} tag, in application. Image won't be deleted from storage device.", "Yes", "No");

            if(answer == true)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    var tagName = arr[1];
                    var tag = conn.Table<Tag>().SingleOrDefault(t => t.Name == tagName);

                    if (tag == null)
                    {
                        await DisplayAlert("Oops..", "Tag required to identify image not found. Operation aborted.", "Ok");
                        return;
                    }

                    var photoPath = arr[0];
                    var photo = conn.Table<Photo>().SingleOrDefault(p => p.Path == photoPath && p.TagId == tag.Id);

                    if (photo != null)
                    {
                        var res = conn.Delete<Photo>(photo.Id);
                        if (res > 0)
                        {
                            await DisplayAlert("Success", "Reference to the image cleared.", "Ok");
                            InitEventsInCalendar();
                        }
                        else
                        {
                            await DisplayAlert("Oops..", "Problem with clearing reference occured :(", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Oops..", "Clearing reference failed. Image record was not found in the database.", "Ok");
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