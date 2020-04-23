using docAndCom.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        private async void GeneratePdf_Clicked(object sender, EventArgs e)
        {
            var tag = tagPicker.SelectedItem;
            var docScheme = docTypePicker.SelectedItem;

            if (tag == null)
            {
                await DisplayAlert("Oops..", "It seems that you did not choose tag. Document creation aborted.", "Ok");
                return;
            } else if (docScheme == null)
            {
                await DisplayAlert("Oops..", "Document scheme type not chosen. Document creation aborted.", "Ok");
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
                (photos, tagName, fileName, docScheme.ToString());

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Models.Document>();

                Models.Document doc = new Models.Document()
                {
                    FileName = fileName,
                    Path = filePath,
                    GeneratedOn = createdOn
                };

                var res = conn.Insert(doc);

                if (res > 0)
                {
                    await DisplayAlert("Success", "Document generated. To see the results, return to previous page using back arrow from the toolbar.", "Great!");
                } 
                else
                {
                    if(File.Exists(filePath))
                    {
                        await DisplayAlert("Oops..", "Document reference not saved, generated file is available at:" + filePath, "Ok");
                    } else
                    {
                        await DisplayAlert("Oops..", "Document reference not saved, file not generated.", "Ok");
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
                    await DisplayAlert("Oops..", "You don't have tag with at least 3 images to perform this operation.", "Got it");

                    createDocBtn.IsEnabled = false;
                }

                tagPicker.ItemsSource = tagsList;
            }
        }
    }
}