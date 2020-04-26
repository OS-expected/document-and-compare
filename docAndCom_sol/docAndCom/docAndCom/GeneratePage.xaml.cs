
using docAndCom.Models;
using docAndCom.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static docAndCom.Helpers.ShortenInvokes;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneratePage : ContentPage
    {
        public GeneratePage()
        {
            InitializeComponent();

            ListAvailableDocuments();
        }

        protected override void OnAppearing()
        {
            ListAvailableDocuments();
        }

        private void GenerateNewDocBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GeneratePdfPage());
        }

        private void ListAvailableDocuments()
        {
            emptyDocListInformation.IsVisible = false;

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Document>();

                var documents = conn.Table<Document>().ToList();

                List<DocumentViewModel> genDocsList = new List<DocumentViewModel>();

                foreach (var doc in documents)
                {
                    var tmp = new DocumentViewModel()
                    {
                        Id = doc.Id,
                        FileName = doc.FileName,
                        Path = doc.Path,
                        GeneratedOn = doc.GeneratedOn.ToString(GetResourceString("dateStringFormat"))
                    };

                    if (File.Exists(doc.Path))
                    {
                        // show button to open & hide label
                        tmp.IsExisting = true;
                        
                    } else
                    {
                        // show label
                        tmp.IsExisting = false;
                    }

                    if(genDocsList.Contains(tmp) == false)
                    {
                        genDocsList.Add(tmp);
                    }

                    if(genDocsList.Count <= 0)
                    {
                        emptyDocListInformation.IsVisible = true;
                    }
                }

                docsListView.ItemsSource = genDocsList;
            }
        }

        private void OpenGeneratedFileBtn_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var doc = button.BindingContext as DocumentViewModel;

            DependencyService.Get<IFileOpener>().OpenFileByGivenPath(doc.Path);
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var doc = button.BindingContext as DocumentViewModel;

            var deleteAlertConfirmationMsg = GetResourceString("deleteFileConfirmationText");
            deleteAlertConfirmationMsg = deleteAlertConfirmationMsg.Replace("<%fileName%>", doc.FileName);

            var result = await DisplayAlert(GetResourceString("areYouSureText"),
                deleteAlertConfirmationMsg, 
                GetResourceString("YesText"),
                GetResourceString("NoText"));

            if (result == true)
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    if (File.Exists(doc.Path))
                    {
                        result = await DisplayAlert(GetResourceString("questionText"),
                            GetResourceString("deleteFileFromStorageText"),
                            GetResourceString("YesText"),
                            GetResourceString("NoText"));

                        if(result == true)
                        {
                            File.Delete(doc.Path);
                        }
                    }

                    // delete ref from dB
                    var deleteRefResult = conn.Delete<Document>(doc.Id);

                    if (deleteRefResult > 0)
                    {
                        await DisplayAlert(GetResourceString("SuccessText"),
                            GetResourceString("operationSuccessfulText"),
                            GetResourceString("OkText"));
                    }
                }

                ListAvailableDocuments();
            }
        }
    }
}