using docAndCom.Models;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace docAndCom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            LoadPreferences();
        }

        private void LoadPreferences()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Preference>();

                var langRes = conn.Table<Preference>().FirstOrDefault(p => p.Key == "lang");

                if (langRes.Value == "en-US")
                {
                    languagePicker.SelectedIndex = 0;
                } 
                else if (langRes.Value == "pl-PL")
                {
                    languagePicker.SelectedIndex = 1;
                }
            }

        }

        private void LanguageSelectedEvent(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedVal = picker.SelectedItem.ToString();
            string resKey = string.Empty;

            if (selectedVal == "English")
            {
                resKey = "en-US";
            } 
            else if (selectedVal == "Polish")
            {
                resKey = "pl-PL";
            }
            
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Preference>();

                var res = conn.Table<Preference>().FirstOrDefault(p => p.Key == "lang");

                if (res != null)
                {
                    res.Value = resKey;
                    conn.Update(res);
                } 
                else
                {
                    Preference pref = new Preference()
                    {
                        Key = "lang",
                        Value = resKey
                    };

                    conn.Insert(pref);
                }
            }

            var newCulture = new CultureInfo(resKey, false);

            App.ResLoader.SetCultureInfo(newCulture);
        }
    }
}