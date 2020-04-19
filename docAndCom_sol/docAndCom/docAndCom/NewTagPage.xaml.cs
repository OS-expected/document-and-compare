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
    public partial class NewTagPage : ContentPage
    {
        public NewTagPage()
        {
            InitializeComponent();
        }

        private void CreateTagBtn_Clicked(object sender, EventArgs e)
        {
            Tag tag = new Tag()
            {
                Name = tagEntry.Text
            };

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Tag>();

                var numberOfRows = conn.Insert(tag);

                if (numberOfRows > 0) {
                    DisplayAlert("Success", "Tag successfuly added.", "Great");
                } else
                {
                    DisplayAlert("Failure", "Tag not inserted.", "Ok");
                }
            }
        }
    }
}