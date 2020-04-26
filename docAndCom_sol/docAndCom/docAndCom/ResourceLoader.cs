using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace docAndCom
{
    public class ResourceLoader : INotifyPropertyChanged
    {
        private readonly ResourceManager manager;
        private CultureInfo cultureInfo;

        public ResourceLoader(ResourceManager resourceManager)
        {
            manager = resourceManager;
            Instance = this;
            cultureInfo = CultureInfo.CurrentUICulture;
        }

        public static ResourceLoader Instance { get; private set; }

        public string GetString(string resourceName)
        {
            string stringRes = this.manager.GetString(resourceName, this.cultureInfo);
            return stringRes;
        }

        public string this[string key] => this.GetString(key);

        public void SetCultureInfo(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public CultureInfo GetCultureInfo()
        {
            return cultureInfo;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
