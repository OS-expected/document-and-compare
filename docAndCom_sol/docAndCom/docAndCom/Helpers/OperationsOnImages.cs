using Xamarin.Forms;

namespace docAndCom.Helpers
{
    public static class OperationsOnImages
    {
        public static void Open(object sender)
        {
            string path = ((Button)sender).BindingContext as string;

            DependencyService.Get<IFileOpener>().OpenFileByGivenPath(path);
        }
    }
}
