using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using DownloadsCleanerCL;
using System.IO;

namespace DownloadsCleanerService
{
    public partial class DownloadsCleanerService : ServiceBase
    {

        private string path;

        private ServiceMainSettings serviceMainSettings;

        private int mainSettingsArg;

        private ServiceSettings serviceSettings;

        private int serviceSettingsArg;

        private List<MyFile> Files;

        private double TotalSize;

        public DownloadsCleanerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            serviceMainSettings = (ServiceMainSettings)Enum.Parse(typeof(ServiceMainSettings), args[0]);
            mainSettingsArg = int.Parse(args[1]);
            serviceSettings = (ServiceSettings)Enum.Parse(typeof(ServiceSettings), args[2]);
            serviceSettingsArg = int.Parse(args[3]);
            path = DownloadsFolder.getPath();
            DirectoryInfo downloadsInfo = new DirectoryInfo(path);
            var files1 = downloadsInfo.GetFiles();
            foreach (var file in files1)
            {
                Files.Add(new MyFile(file));
            }
            var dirs = downloadsInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                Files.Add(new MyFile(dir));
            }
            foreach (var file in Files)
            {
                TotalSize += file.Size;
            }
            TotalSize /= 1024.0;
        }

        protected override void OnStop()
        {
        }
    }
}
