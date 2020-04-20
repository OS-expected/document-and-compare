using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewDocumentPage : ContentPage
    {
        public NewDocumentPage()
        {
            InitializeComponent();

            FeedTags();
        }

        async void TakePhoto_Clicked(object sender, System.EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private void FeedTags()
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
                foreach (var tag in tagsList)
                {
                    if(tagsList.Contains(tag) == false)
                    {
                        tagsList.Add(tags[0].Name);
                    }
                }

                tagPicker.ItemsSource = tagsList;
            }
        }
    }
}