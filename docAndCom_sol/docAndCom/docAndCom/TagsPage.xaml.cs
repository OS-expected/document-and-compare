using docAndCom.Models;
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
                    emptyTagsLabel.IsVisible = true;
                } else
                {
                    emptyTagsLabel.IsVisible = false;
                }

                tagsListView.ItemsSource = tags;
            }
        }
    }
}