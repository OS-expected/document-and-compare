using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace docAndCompare
{
    [Activity(Label = "TagsActivity")]
    public class TagsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_tags);

            Button menuBtn = FindViewById<Button>(Resource.Id.menuBtn);
            menuBtn.Click += menuBtn_Click;
        }

        private void menuBtn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}