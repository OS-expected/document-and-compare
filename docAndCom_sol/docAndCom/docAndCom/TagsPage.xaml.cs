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
    public partial class TagsPage : ContentPage
    {
        public TagsPage()
        {
            InitializeComponent();

            DisplayTags();
        }

        private void DisplayTags()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Tag>();

                var tags = conn.Table<Tag>().ToList();

                tagsListView.ItemsSource = tags;
            }

        }
    }
}