﻿using docAndCom.Models;
using System;
using System.Collections.Generic;
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

        private async void ToolbarItem_Document_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewDocumentPage());
        }

        private void OpenImageBtn_Clicked(object sender, EventArgs e)
        {
            string path = ((Button)sender).BindingContext as string;

            DependencyService.Get<IFileOpener>().OpenFileByGivenPath(path);
        }

        private async void ClearRefBtn_Clicked(object sender, EventArgs e)
        {
            string clearRefData = ((Button)sender).BindingContext as string;
            string[] arr = clearRefData.Split(new[] { '|' }, 2); // 0 -> path, 1 -> tag

            bool answer = await DisplayAlert("Are you sure?", $"This operation removes reference to the documented image from {arr[1]} tag, in application. Image won't be deleted from storage device.", "Yes", "No");

            if(answer == true)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    var tagId = conn.Table<Tag>().FirstOrDefault(t => t.Name == arr[1]);

                    if (tagId == null)
                    {
                        await DisplayAlert("Failure", "Tag not found. Operation aborted.", "Ok");
                        return;
                    }

                    var photoId = conn.Table<Photo>().FirstOrDefault(p => p.Path == arr[0] && p.TagId == tagId.Id);

                    if (photoId != null)
                    {
                        var res = conn.Delete(photoId.Id);
                        if (res > 0)
                        {
                            InitEventsInCalendar();
                            await DisplayAlert("Success", "Clear reference operation succeded.", "Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Failure", "Clearing reference failed. Image record was not found in the database.", "Ok");
                    }
                }
            }      
        }

        private void InitEventsInCalendar()
        {
            Events = new EventCollection();

            SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH);

            conn.CreateTable<Photo>();

            var photos = conn.Table<Photo>().ToList();

            var days = (from table in conn.Table<Photo>()
                  select table.CreatedOn).Distinct().ToList();

            foreach (var day in days)
            {
                List<EventModel> list = new List<EventModel>();

                foreach (var photoObj in photos)
                {
                    if(photoObj.CreatedOn == day)
                    {
                        list.Add(new EventModel()
                        {
                            Tag = tagName,
                            ImagePath = photoObj.Path,
                            ClearRefData = photoObj.Path + "|" + tagName
                        };

                        list.Add(eventModel);
                    }
                }

                conn.Dispose();

                Events.Add(day, list);
            }

            calendarRef.Events = Events;
        }
    }
}