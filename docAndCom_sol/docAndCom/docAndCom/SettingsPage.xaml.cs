using docAndCom.Models;
using System;
using System.Globalization;
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

            var tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) => ToggleExtraImageCopy();
            extraCopyToggler.GestureRecognizers.Add(tgr);
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

                var prefRes = conn.Table<Preference>().FirstOrDefault(p => p.Key == "numOfCol");

                if (prefRes.Value == "3")
                {
                    columnPicker.SelectedIndex = 0;
                }
                else if (prefRes.Value == "4")
                {
                    columnPicker.SelectedIndex = 1;
                }
                else if (prefRes.Value == "5")
                {
                    columnPicker.SelectedIndex = 2;
                }

                var extraImageRes = conn.Table<Preference>().FirstOrDefault(p => p.Key == "saveCopyToAlbum");

                if (extraImageRes.Value == "false")
                {
                    extraCopyToggler.Text = "&#xf204;";
                    extraCopyToggler.TextColor = Color.FromHex("#881452");
                }
                else if (extraImageRes.Value == "true")
                {
                    extraCopyToggler.Text = "&#xf205;";
                    extraCopyToggler.TextColor = Color.FromHex("#4ADA76");
                }
            }

        }

        private void ColumnNumberSelectedEvent(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedVal = picker.SelectedItem.ToString().Substring(0, 1);

            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Preference>();

                var res = conn.Table<Preference>().FirstOrDefault(p => p.Key == "numOfCol");

                Preference pref = new Preference()
                {
                    Key = "numOfCol",
                    Value = "3"
                };

                if(res == null && selectedVal != res.Value)
                {
                    conn.Insert(pref);
                } 
                else if (selectedVal != res.Value)
                {
                    res.Value = selectedVal;
                    conn.Update(res);
                }
            }
        }

        private void ToggleExtraImageCopy()
        {
            using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Preference>();

                var res = conn.Table<Preference>().FirstOrDefault(p => p.Key == "saveCopyToAlbum");

                if(res.Value == "false")
                {
                    res.Value = "true";
                    extraCopyToggler.Text = "&#xf205;";
                    extraCopyToggler.TextColor = Color.FromHex("#4ADA76");
                } 
                else if (res.Value == "true")
                {
                    res.Value = "false";
                    extraCopyToggler.Text = "&#xf204;";
                    extraCopyToggler.TextColor = Color.FromHex("#881452");
                }

                conn.Update(res);
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