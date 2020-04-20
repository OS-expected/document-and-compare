using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace docAndCom
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Repository_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri(@"https://github.com/trolit/document-and-compare");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private async void TagsBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TagsPage());
        }

        private async void DocBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DocumentPage());
        }
    }
}
