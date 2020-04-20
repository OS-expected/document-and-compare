using docAndCom.Models;
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

        private void InitEventsInCalendar()
        {
            Events = new EventCollection();

            SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH);

            conn.CreateTable<Photo>();

            var photos = conn.Table<Photo>().ToList();

            var days = (from table in conn.Table<Photo>()
                  select table.CreatedOn).Distinct().ToList();

            conn.Dispose();

            foreach (var day in days)
            {
                List<EventModel> list = new List<EventModel>();

                foreach (var photoObj in photos)
                {
                    if(photoObj.CreatedOn == day)
                    {
                        list.Add(new EventModel()
                        {
                            Tag = "Test",
                            ImagePath = photoObj.Path
                        });
                    }
                }

                Events.Add(day, list);
            }

            calendarRef.Events = Events;
        }
    }
}