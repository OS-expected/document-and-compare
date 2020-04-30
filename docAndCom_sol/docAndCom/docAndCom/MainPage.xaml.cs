using System;
using System.ComponentModel;
using System.Threading.Tasks;
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

        protected override void OnAppearing()
        {
            aiLayout.IsVisible = false;
            ai.IsRunning = false;
        }

        private async void Repository_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri(@"https://github.com/trolit/document-and-compare");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private async void TagsBtn_Clicked(object sender, EventArgs e)
        {
            ToggleActivityIndicator();
            await Task.Delay(1000);
            await Navigation.PushAsync(new TagsPage());
        }

        private async void DocBtn_Clicked(object sender, EventArgs e)
        {
            ToggleActivityIndicator();
            await Task.Delay(1200);
            await Navigation.PushAsync(new DocumentPage());
        }

        private async void GenBtn_Clicked(object sender, EventArgs e)
        {
            ToggleActivityIndicator();
            await Task.Delay(1000);
            await Navigation.PushAsync(new GeneratePage());
        }

        private async void About_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void GetStartedBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GetStartedPage());
        }

        private void ToggleActivityIndicator()
        {
            ai.IsRunning = true;
            aiLayout.IsVisible = true;
        }
    }
}
