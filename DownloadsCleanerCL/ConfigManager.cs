using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace DownloadsCleanerCL
{
    public enum ServiceMainSettings
    {
        Interval = 0,
        Limit = 1
    }

    public enum ServiceSettings
    {
        Oldest = 0,
        Biggest = 1,
        OlderThan = 2,
        BiggerThan = 3
    }

    public class ConfigManager
    {
        public ServiceMainSettings MainSettings { get; set; }

        public ServiceSettings ServiceSettings { get; set; }

        public int Files { get; set; }

        public int Size { get; set; }

        public int Days { get; set; }

        public int Limit { get; set; }

        public int OlderThan { get; set; }

        public void GetSettings()
        {
            NameValueCollection valueCollection = ConfigurationManager.AppSettings;
            MainSettings = (ServiceMainSettings)Enum.Parse(typeof(ServiceMainSettings), valueCollection.Get("ServiceMainSettings"));
            ServiceSettings = (ServiceSettings)Enum.Parse(typeof(ServiceSettings), valueCollection.Get("ServiceSettings"));
            Files = int.Parse(valueCollection.Get("FilesNumber"));
            Size = int.Parse(valueCollection.Get("Size"));
            Days = int.Parse(valueCollection.Get("Days"));
            Limit = int.Parse(valueCollection.Get("Limit"));
            OlderThan = int.Parse(valueCollection.Get("OlderThan"));
        }

        public async Task SetSettingsAsync()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["ServiceMainSettings"].Value = MainSettings.ToString();
            configuration.AppSettings.Settings["ServiceSettings"].Value = ServiceSettings.ToString();
            configuration.AppSettings.Settings["FilesNumber"].Value = Files.ToString();
            configuration.AppSettings.Settings["Size"].Value = Size.ToString();
            configuration.AppSettings.Settings["Days"].Value = Days.ToString();
            configuration.AppSettings.Settings["Limit"].Value = Limit.ToString();
            configuration.AppSettings.Settings["OlderThan"].Value = OlderThan.ToString();
            configuration.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection(configuration.AppSettings.SectionInformation.Name);
        }
    }
}
