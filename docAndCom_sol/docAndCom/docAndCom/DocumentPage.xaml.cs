using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentPage : ContentPage
    {
        public DocumentPage()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Document_Activated(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewDocumentPage());
        }
    }
}