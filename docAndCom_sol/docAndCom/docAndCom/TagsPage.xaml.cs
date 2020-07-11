using docAndCom.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static docAndCom.Helpers.ShortenInvokes;

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

        private async void OnAccordingImagesClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var tag = button.BindingContext as Tag;
            await Navigation.PushAsync(new CorrespondingPhotosPage(tag.Id, tag.Name));
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert(GetResourceString("areYouSureText"),
                GetResourceString("deleteThatTag"),
                GetResourceString("YesText"),
                GetResourceString("NoText"));

            if(result == true)
            {
                var button = sender as Button;
                var tag = button.BindingContext as Tag;

                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    int tagId = GetTagIdByName(tag.Name);
                    var numberOfDocuments = conn.Table<Photo>().Count(p => p.TagId == tagId);
                    int timesDeleted = 0;

                    if (numberOfDocuments > 0)
                    {
                        string alert_phrase1 = numberOfDocuments == 1 ? GetResourceString("singleImage") : $"{numberOfDocuments} " + GetResourceString("multipleImages");
                        string alert_phrase2 = numberOfDocuments == 1 ? GetResourceString("singularVariety") : GetResourceString("pluralVariety");

                        string deleteImagesAlertDesc = GetResourceString("tagRemoveAlertDesc");
                        deleteImagesAlertDesc = deleteImagesAlertDesc.Replace("<%tagName%>", tag.Name);
                        deleteImagesAlertDesc = deleteImagesAlertDesc.Replace("<%alert_phrase1%>", alert_phrase1);
                        deleteImagesAlertDesc = deleteImagesAlertDesc.Replace("<%alert_phrase2%>", alert_phrase2);

                        result = await DisplayAlert(GetResourceString("warningText"),
                            deleteImagesAlertDesc,
                            GetResourceString("YesText"),
                            GetResourceString("NoText"));

                        if(result == true)
                        {
                            var photos = conn.Table<Photo>().Where(p => p.TagId == tagId).ToList();

                            foreach (var photo in photos)
                            {
                                if (File.Exists(photo.Path))
                                {
                                    File.Delete(photo.Path);
                                    timesDeleted++;
                                }

                                conn.Delete<Photo>(photo.Id);
                            }

                            string successAlertMsg = string.Empty;

                            if(timesDeleted == 1)
                            {
                                successAlertMsg = GetResourceString("singleImageDeletedText");
                            } else if (timesDeleted <= 4)
                            {
                                successAlertMsg = GetResourceString("upToFourImagesDeletedText");
                            } else if (timesDeleted > 4)
                            {
                                successAlertMsg = GetResourceString("MoreThanFourImagesDeletedText");
                            }

                            await DisplayAlert(GetResourceString("SuccessText"),
                                $"{timesDeleted} " + successAlertMsg,
                                GetResourceString("OkText"));
                        }
                    }

                    conn.Delete<Tag>(tagId);
                }

                DisplayTags();
            }
        }

        private async void ToolbarItem_AddTag_Activated(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync(GetResourceString("createTagTitleText"),
                GetResourceString("createTagDesc"),
                GetResourceString("createText"),
                GetResourceString("cancelText"), null, 35);

            if (string.IsNullOrEmpty(result) == false && result.Length >= 3)
            {
                var regexItem = new Regex("^[a-zA-Z0-9-_]*$");
                if (regexItem.IsMatch(result) == false)
                {
                    await DisplayAlert(GetResourceString("OopsText"),
                        GetResourceString("tagForbiddenChars"),
                        GetResourceString("OkText"));
                    return;
                }

                Tag tag = new Tag()
                {
                    Name = result
                };

                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Tag>();

                    var isExisting = conn.Table<Tag>().FirstOrDefault(t => t.Name == tag.Name);

                    if(isExisting != null)
                    {
                        await DisplayAlert(GetResourceString("OopsText"), 
                            $"{GetResourceString("tagDuplicate")} {tag.Name}.",
                            GetResourceString("OkText"));
                        return;
                    }

                    var numberOfRows = conn.Insert(tag);

                    if (numberOfRows > 0)
                    {
                        await DisplayAlert(GetResourceString("SuccessText"),
                            GetResourceString("tagAdded"),
                            GetResourceString("greatText"));
                    }
                    else
                    {
                        await DisplayAlert(GetResourceString("OopsText"),
                            GetResourceString("tagNotSavedError"),
                            GetResourceString("OkText"));
                    }
                }
                DisplayTags();
            } else if (result != null && result.Length < 3)
            {
                await DisplayAlert(GetResourceString("OopsText"),
                    GetResourceString("tagNotEnoughChars"),
                    GetResourceString("OkText"));
            }
        }

        private int GetTagIdByName(string name)
        {
            Tag tag;
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                tag = conn.Table<Tag>().FirstOrDefault(t => t.Name == name);
            }
            return tag.Id;
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