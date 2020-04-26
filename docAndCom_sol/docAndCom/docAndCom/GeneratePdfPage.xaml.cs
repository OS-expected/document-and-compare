using docAndCom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static docAndCom.Helpers.ShortenInvokes;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneratePdfPage : ContentPage
    {
        public GeneratePdfPage()
        {
            InitializeComponent();

            FeedTagPicker();
        }

        protected override void OnAppearing()
        {
            docTypePicker.ItemsSource[0] = GetResourceString("generateFilepickerList");
            docTypePicker.ItemsSource[1] = GetResourceString("generateFilepickerTabular");
        }

        private async void GeneratePdf_Clicked(object sender, EventArgs e)
        {
            ai.IsRunning = true;
            aiLayout.IsVisible = true;

            var tag = tagPicker.SelectedItem;
            var docScheme = docTypePicker.SelectedIndex;

            if (tag == null)
            {
                await DisplayAlert(GetResourceString("OopsText"),
                    GetResourceString("tagNotChosenText"),
                    GetResourceString("OkText"));
                return;
            } else if (docScheme != 0 && docScheme != 1)
            {
                await DisplayAlert(GetResourceString("OopsText"),
                    GetResourceString("fileSchemeNotChosenText"),
                    GetResourceString("OkText"));
                return;
            }

            var tagName = tag.ToString();
            int numberOfImages = 0;
            var createdOn = DateTime.UtcNow;
            List<Photo> photos = new List<Photo>();

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Models.Document>();

                int tagId = conn.Table<Tag>().SingleOrDefault(t => t.Name == tagName).Id;

                numberOfImages = conn.Table<Photo>().Where(p => p.TagId == tagId).Count();

                photos = conn.Table<Photo>().Where(p => p.TagId == tagId).ToList();
            }

            var fileName = $"{tagName}_{createdOn:dd-MM-yyyy}_{createdOn:HH-mm-ss}.pdf";

            var filePath = DependencyService.Get<IFileSaver>().GeneratePdfFile
                (photos, tagName, fileName, docScheme);

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                Models.Document doc = new Models.Document()
                {
                    FileName = fileName,
                    Path = filePath,
                    GeneratedOn = createdOn
                };

                var res = conn.Insert(doc);

                ai.IsRunning = false;
                aiLayout.IsVisible = false;

                if (res > 0)
                {
                    await DisplayAlert(GetResourceString("SuccessText"),
                        GetResourceString("fileGeneratedText"), 
                        GetResourceString("greatText"));
                } 
                else
                {
                    if(File.Exists(filePath))
                    {
                        var alertMsg = GetResourceString("fileRefNotSavedButFileGenerated");
                        alertMsg = alertMsg.Replace("<%filePath%>", filePath);

                        await DisplayAlert(GetResourceString("OopsText"),
                            alertMsg, 
                            GetResourceString("OkText"));
                    } else
                    {
                        await DisplayAlert(GetResourceString("OopsText"),
                            GetResourceString("fileRefNotSavedNotGenerated"), 
                            GetResourceString("OkText"));
                    }
                }
            }
        }

        private async void FeedTagPicker()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Tag>();

                var tags = conn.Table<Tag>().ToList();

                var tagsList = new List<string>();
                foreach (var tag in tags)
                {
                    var numberOfImages = conn.Table<Photo>().Where(p => p.TagId == tag.Id).Count();

                    if (tagsList.Contains(tag.Name) == false && numberOfImages > 2)
                    {
                        tagsList.Add(tag.Name);
                    }
                }

                if(tagsList.Count <= 0)
                {
                    await DisplayAlert(GetResourceString("OopsText"),
                        GetResourceString("notEnoughImages"),
                        GetResourceString("OkText"));

                    createDocBtn.IsEnabled = false;
                }

                tagPicker.ItemsSource = tagsList;
            }
        }
    }
}