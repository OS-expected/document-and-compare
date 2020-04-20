using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using Android.Webkit;
using docAndCom.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidCommandsActivity))]
namespace docAndCom.Droid
{
    [Activity(Label = "AndroidCommandsActivity")]
    public class AndroidCommandsActivity : Activity, IFileOpener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public void OpenFileByGivenPath(string sourcePath)
        {
            Java.IO.File file = new Java.IO.File(sourcePath);
            Android.Net.Uri uri = FileProvider.GetUriForFile(Application.Context, "com.companyname.docandcom.fileprovider", file);
            string extension = MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
            string mimeType = MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, mimeType);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.NoHistory);
            var chooserIntent = Intent.CreateChooser(intent, "Open PDF");
            chooserIntent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            Application.Context.StartActivity(chooserIntent);
        }
    }
}