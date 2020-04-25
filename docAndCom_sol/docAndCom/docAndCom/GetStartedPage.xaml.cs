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
    public partial class GetStartedPage : ContentPage
    {
        public GetStartedPage()
        {
            InitializeComponent();
        }

        private void TagBtn_Clicked(object sender, EventArgs e)
        {
            step1_tag.IsVisible = true;
            step2_doc.IsVisible = false;
            step3_gen.IsVisible = false;
        }

        private void DocBtn_Clicked(object sender, EventArgs e)
        {
            step1_tag.IsVisible = false;
            step2_doc.IsVisible = true;
            step3_gen.IsVisible = false;
        }

        private void GenBtn_Clicked(object sender, EventArgs e)
        {
            step1_tag.IsVisible = false;
            step2_doc.IsVisible = false;
            step3_gen.IsVisible = true;
        }
    }
}