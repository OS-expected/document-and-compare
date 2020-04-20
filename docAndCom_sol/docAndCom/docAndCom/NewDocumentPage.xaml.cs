using docAndCom.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDocumentPage : ContentPage
    {
        private string pathToImage = "";

        private string pickedTag = "";

        // private string pickedDate = "";

        public NewDocumentPage()
        {
            InitializeComponent();

            FeedTagPicker();
        }

        async void TakePhoto_Clicked(object sender, System.EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var now = DateTime.UtcNow;

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "docAndComparePhotos",
                Name = $"dac_{now:dd-MM-yyyy}_{now:HH_mm}.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            pathToImage = file.Path.ToString();

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void Gallery_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("no upload", "picking a photo is not supported", "ok");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() => file.GetStream());

            pathToImage = file.Path.ToString();
        }

        private void FeedTagPicker()
        {
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Tag>();

                var tags = conn.Table<Tag>().ToList();

                if (tags.Count == 0)
                {
                    tagPicker.IsEnabled = false;
                    tagPicker.Title = "No tags specified!";
                    // emptyTagsLabel.IsVisible = true;
                }
                else
                {
                    // emptyTagsLabel.IsVisible = false;
                }

                var tagsList = new List<string>();
                foreach (var tag in tags)
                {
                    if(tagsList.Contains(tag.Name) == false)
                    {
                        tagsList.Add(tag.Name);
                    }
                }

                tagPicker.ItemsSource = tagsList;
            }
        }

        private async void DocumentBtn_Clicked(object sender, EventArgs e)
        {
            pickedTag = tagPicker.SelectedItem.ToString();

            if (string.IsNullOrEmpty(pickedTag))
            {
                await DisplayAlert("Operation aborted", "Tag not picked.", "Ok");
                return;
            }

            SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH);

            var id = conn.Table<Tag>().SingleOrDefault(t => t.Name == pickedTag).Id;

            if (id <= 0)
            {
                conn.Dispose();
                await DisplayAlert("Operation aborted", "Tag does not exist in database.", "Ok");
                return;
            }

            conn.CreateTable<Photo>();

            Photo photo = new Photo()
            {
                Path = pathToImage,
                CreatedOn = documentDatePicker.Date,
                TagId = id
            };

            var numberOfRows = conn.Insert(photo);

            conn.Dispose();

            if (numberOfRows > 0)
            {
                await DisplayAlert("Success", "Image successfuly documented in the app.", "Great!");
            }
            else
            {
                await DisplayAlert("Failure", "Image not documented in the app, try again!", "Ok");
            }
        }
    }
}