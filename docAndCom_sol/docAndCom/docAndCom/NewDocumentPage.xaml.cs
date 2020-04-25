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

        public NewDocumentPage()
        {
            InitializeComponent();

            FeedTagPicker();
        }

        async void TakePhoto_Clicked(object sender, System.EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Oops..", "Camera is not available :(", "Ok.");
                return;
            }

            var now = DateTime.UtcNow;

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "docAndComparePhotos",
                Name = $"dac_{now:dd-MM-yyyy}_{now:HH_mm_ss}.jpg"
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
                await DisplayAlert("Oops..", "Picking a photo is not supported :(", "Ok");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() => file.GetStream());

            pathToImage = file.Path.ToString();
        }

        private async void FeedTagPicker()
        {
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Tag>();

                var tags = conn.Table<Tag>().ToList();

                if (tags.Count == 0)
                {
                    var res = await DisplayAlert("Oops..", "It looks like you did not create tag required to perform document operation. Would you like to be redirected to the tags page?", "Yes", "No");
                    
                    if(res == true)
                    {
                        await Navigation.PushAsync(new TagsPage());
                    } 
                    else
                    {
                        tagPicker.IsEnabled = false;
                    }
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
            var pickedTag = tagPicker.SelectedItem;

            if (pickedTag == null)
            {
                await DisplayAlert("Oops..", "Tag not picked!", "Ok");
                return;
            }

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH)) 
            {
                var id = conn.Table<Tag>().SingleOrDefault(t => t.Name == pickedTag.ToString()).Id;

                if (id <= 0)
                {
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

                if (numberOfRows > 0)
                {
                    await DisplayAlert("Success", "Image successfuly documented in the app. You can now return to the calendar.", "Great!");
                }
                else
                {
                    await DisplayAlert("Failure", "Image not documented in the app, try again!", "Ok");
                }
            }
        }
    }
}