using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Webkit;
using docAndCom.Droid;
using docAndCom.Models;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidCommandsActivity))]
namespace docAndCom.Droid
{
    [Activity(Label = "AndroidCommandsActivity")]
    public class AndroidCommandsActivity : Activity, IFileOpener, IFileSaver
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
            var chooserIntent = Intent.CreateChooser(intent, "Open file");
            chooserIntent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            Application.Context.StartActivity(chooserIntent);
        }

        public string GeneratePdfFile(List<Photo> photos, string tag, string fileName)
        {
            string root = null;

            if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.WriteExternalStorage)
                != Permission.Granted)
            {
                ActivityCompat.RequestPermissions((Android.App.Activity)
                    Application.Context, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
            }

            if (Android.OS.Environment.IsExternalStorageEmulated)
            {
                root = Android.OS.Environment.ExternalStorageDirectory.ToString();
            }
            else
            {
                root = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            }

            Java.IO.File myDir = new Java.IO.File(root + "/DocAndComPDFs");
            if (myDir.Exists() == false)
            {
                myDir.Mkdir();
            }

            var _path = Path.Combine(root, "DocAndComPDFs", fileName);

            FileStream fs = new FileStream(_path, FileMode.Create, FileAccess.Write);

            var doc = new iTextSharp.text.Document();

            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();

            doc.Add(new iTextSharp.text.Paragraph("Hello World"));

            doc.Close();

            return _path;
        }
    }
}