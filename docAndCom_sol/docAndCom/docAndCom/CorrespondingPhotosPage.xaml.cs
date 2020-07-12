using docAndCom.Helpers;
using docAndCom.Models;
using docAndCom.ViewModels;
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
    public partial class CorrespondingPhotosPage : ContentPage
    {
        private int _tagId = 0;

        public CorrespondingPhotosPage(int tagId, string tagName)
        {
            InitializeComponent();
            tagLabel.Text = tagName;
            _tagId = tagId;
            SupplyPhotosListView();
        }

        private void SupplyPhotosListView()
        {
            photosListView.ItemsSource = null;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                List<Photo> photos = conn.Table<Photo>().Where(p => p.TagId == _tagId).OrderBy(p => p.CreatedOn).ToList();
                List<CorrespondingPhotosViewModel> photosItemSource = new List<CorrespondingPhotosViewModel>();
                foreach (var photo in photos)
                {
                    photosItemSource.Add(new CorrespondingPhotosViewModel
                    {
                        Id = photo.Id,
                        ImagePath = photo.Path,
                        DateOfCreation = photo.CreatedOn.ToString("dd.MM.yyyy")
                    });
                }

                photosListView.ItemsSource = photosItemSource;
            }
        }

        private void OpenImageBtn_Clicked(object sender, EventArgs e)
        {
            OperationsOnImages.Open(sender);
        }

        private async void DeleteImageBtn_Clicked(object sender, EventArgs e)
        {
            var photo = ((Button)sender).BindingContext as CorrespondingPhotosViewModel;

            var hardDelConfirmationAlertDesc = GetResourceString("hardDeleteAlertNoTag");

            bool answer = await DisplayAlert(GetResourceString("areYouSureText"),
                hardDelConfirmationAlertDesc, GetResourceString("YesText"),
                GetResourceString("NoText"));

            if (answer == true)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    var photoToDelete = conn.Table<Photo>().FirstOrDefault(p => p.Id == photo.Id);

                    if (photoToDelete == null)
                    {
                        await DisplayAlert(GetResourceString("OopsText"),
                            GetResourceString("imageRefNotFoundText"),
                            GetResourceString("OkText"));
                        return;
                    }

                    var res = conn.Delete<Photo>(photoToDelete.Id);

                    if (File.Exists(photoToDelete.Path))
                    {
                        File.Delete(photoToDelete.Path);
                    }

                    string extraNote = GetResourceString("imageLeftText");

                    if (File.Exists(photoToDelete.Path) == false)
                    {
                        extraNote = GetResourceString("imageRemovedText");
                    }

                    SupplyPhotosListView();

                    if (res > 0)
                    {
                        await DisplayAlert(GetResourceString("SuccessText"),
                            GetResourceString("referenceClearedText") + extraNote,
                            GetResourceString("OkText"));
                    }
                    else if (res <= 0)
                    {
                        await DisplayAlert(GetResourceString("OopsText"),
                            GetResourceString("referenceNotClearedText") + extraNote,
                            GetResourceString("OkText"));
                    }
                }
            }

        }


    }
}