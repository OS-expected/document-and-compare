using docAndCom.Models;
using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagsPage : ContentPage
    {
        public TagsPage()
        {
            InitializeComponent();

            DisplayTags();
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Confirmation", "Are you sure, you want to delete that tag?", "Yes", "No");

            if(result == true)
            {
                var button = sender as Button;
                var tag = button.BindingContext as Tag;

                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    var numberOfDocuments = conn.Table<Photo>().Count(p => p.TagId == tag.Id);
                    int timesDeleted = 0;

                    if (numberOfDocuments > 0)
                    {
                        string alert_phrase1 = numberOfDocuments == 1 ? "1 image." : $"{numberOfDocuments} images.";
                        string alert_phrase2 = numberOfDocuments == 1 ? "it" : "them";

                        result = await DisplayAlert("Warning", $"{tag.Name} tag is used by {alert_phrase1} Do you want to remove {alert_phrase2} from storage device?", "Yes", "No");

                        if(result == true)
                        {
                            var photos = conn.Table<Photo>().Where(p => p.TagId == tag.Id).ToList();

                            foreach (var photo in photos)
                            {
                                if (File.Exists(photo.Path))
                                {
                                    File.Delete(photo.Path);
                                    timesDeleted++;
                                }

                                conn.Delete<Photo>(photo.Id);
                            }

                            var note = timesDeleted == 1 ? "image" : "images";
                            await DisplayAlert("Note", $"{timesDeleted} {note} deleted from the device.", "Ok");
                        }
                    }

                    conn.Delete<Tag>(tag.Id);
                }

                DisplayTags();
            }
        }

        private async void ToolbarItem_AddTag_Activated(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("Tag", "Specify name of the tag(3-20 characters)",
                "Create", "Abort", null, 20);

            if (string.IsNullOrEmpty(result) == false && result.Length >= 3)
            {
                Tag tag = new Tag()
                {
                    Name = result

                };

                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Tag>();

                    var numberOfRows = conn.Insert(tag);

                    if (numberOfRows > 0)
                    {
                        await DisplayAlert("Success", "Tag successfuly added.", "Great");
                    }
                    else
                    {
                        await DisplayAlert("Failure", "Tag not inserted.", "Ok");
                    }
                }
                DisplayTags();
            } else if (result != null && result.Length < 3)
            {
                await DisplayAlert("Failure", "Tag not added. Not enough characters.", "Ok");
            }

            // Navigation.PushAsync(new NewTagPage());
        }

        private void DisplayTags()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Tag>();

                var tags = conn.Table<Tag>().ToList();

                if(tags.Count == 0)
                {
                    emptyTagsMsg.IsVisible = true;
                } else
                {
                    emptyTagsMsg.IsVisible = false;
                }

                tagsListView.ItemsSource = tags;
            }
        }
    }
}