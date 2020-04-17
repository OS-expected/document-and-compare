using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using System;
using Android.Widget;
using Android.Content;

namespace docAndCompare
{
    [Activity(Label = "@strings/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Button tagsBtn = FindViewById<Button>(Resource.Id.tagsBtn);
            tagsBtn.Click += TagsBtn_Click;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void TagsBtn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TagsActivity));
            StartActivity(intent);
        }
    }
}