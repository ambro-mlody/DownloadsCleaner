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
using System.Timers;
using System.Configuration;
using System.Collections.Specialized;

namespace DownloadsCleanerService
{
    public partial class DownloadsCleanerService : ServiceBase
    {

        private string path;

        private ServiceMainSettings serviceMainSettings;

        private int mainSettingsArg;

        private ServiceSettings serviceSettings;

        private int serviceSettingsArg;

        private List<MyFile> Files = new List<MyFile>();

        private double TotalSize;

        private FileSystemWatcher watcher;

        static TraceSwitch mySwitch = new TraceSwitch("ServiceTraceSwitch", "TraceSwitch");

        public DownloadsCleanerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if(args.Length == 5)
            {
               
                serviceMainSettings = (ServiceMainSettings)Enum.Parse(typeof(ServiceMainSettings), args[0]);
                mainSettingsArg = int.Parse(args[1]);
                serviceSettings = (ServiceSettings)Enum.Parse(typeof(ServiceSettings), args[2]);
                serviceSettingsArg = int.Parse(args[3]);
                path = args[4];
                ConfigManager.WriteServiceSettings(serviceMainSettings, mainSettingsArg, serviceSettings, serviceSettingsArg, path);
            }
            else
            {
                var manager = ConfigManager.GetServiceSettings();
                serviceMainSettings = manager.MainSettings;
                mainSettingsArg = manager.MainSettingsArg;
                serviceSettings = manager.ServiceSettings;
                serviceSettingsArg = manager.SettingsArg;
                path = manager.Path;
            }

            if (mySwitch.TraceWarning)
            {
                eventLog1.WriteEntry($"{DateTime.Now} - Service Start: {serviceMainSettings}, days/limit: {mainSettingsArg}, {serviceSettings}, files/days/size: {serviceSettingsArg}");
            }

            //path = DownloadsFolder.getPath();
            if(mySwitch.TraceInfo)
            {
                eventLog1.WriteEntry($"{DateTime.Now} - Path to downloads folder: {path}");
            }
            DirectoryInfo downloadsInfo = new DirectoryInfo(path);
            Directory.GetFiles(path);
            var files1 = downloadsInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
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

            if(mySwitch.TraceInfo)
            {
                eventLog1.WriteEntry($"Get Files succesfull. Total Size: {TotalSize}");
            }

            if(serviceMainSettings == ServiceMainSettings.Interval)
            {
                Timer timer = new Timer();
                timer.Interval = 3600000;
                //timer.Interval = 10000;
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = true;
                timer.Start();
                ConfigManager.UpdateSchedule(mainSettingsArg);
            }

            watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Size |
                NotifyFilters.Security;

            watcher.Changed += Watcher_Changed;

            watcher.Created += Watcher_Created;

            watcher.Deleted += Watcher_Deleted;

            watcher.Renamed += Watcher_Renamed;

            watcher.EnableRaisingEvents = true;
            
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
                return;

            
            MyFile file = Files.Find(f => f.Name == e.Name);
            if (!(file is null))
            {
                Files.Remove(file);
                TotalSize -= file.Size / 1024.0;
                if (File.Exists(e.FullPath))
                {
                    file = new MyFile(new FileInfo(e.FullPath));
                    Files.Add(file);
                }
                else if (Directory.Exists(e.FullPath))
                {
                    file = new MyFile(new DirectoryInfo(e.FullPath));
                    Files.Add(file);
                }
                TotalSize += file.Size / 1024.0;

                if (mySwitch.TraceInfo)
                {
                    eventLog1.WriteEntry($"{DateTime.Now} - File {e.Name} was changed.");
                }
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            MyFile file = Files.Find(f => f.Name == e.OldName);
            if(!(file is null))
            {
                Files.Remove(file);
                file.Name = e.Name;
                Files.Add(file);
                if (mySwitch.TraceInfo)
                {
                    eventLog1.WriteEntry($"{DateTime.Now} - File {e.OldName} was renamed to {e.Name}.");
                }
            }
            
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            MyFile file = Files.Find(f => f.Name == e.Name);
            if(!(file is null))
            {
                Files.Remove(file);
                TotalSize -= file.Size / 1024.0;
                if (mySwitch.TraceInfo)
                {
                    eventLog1.WriteEntry($"{DateTime.Now} - File {e.Name} was deleted.");
                }
            }
            
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            MyFile file;
            if (File.Exists(e.FullPath))
            {
                file = new MyFile(new FileInfo(e.FullPath));
                Files.Add(file);
                TotalSize += file.Size / 1024.0;
            }
            else if (Directory.Exists(e.FullPath))
            {
                file = new MyFile(new DirectoryInfo(e.FullPath));
                Files.Add(file);
                TotalSize += file.Size / 1024.0;
            }

            if (mySwitch.TraceInfo)
            {
                eventLog1.WriteEntry($"{DateTime.Now} - File {e.Name} was created.");
            }

            if (serviceMainSettings == ServiceMainSettings.Limit)
            {
                if (TotalSize > mainSettingsArg)
                {
                    if(mySwitch.TraceWarning)
                    {
                        eventLog1.WriteEntry($"{DateTime.Now} - Limit exceeded. Total Size: {TotalSize} MB, limit: {mainSettingsArg} MB");
                    }
                    HandleSettings();
                }
            }
        }

        private async void HandleSettings()
        {
            switch (serviceSettings)
            {
                case ServiceSettings.Oldest:
                    var f = await FileSorter.SortByDateAsync(Files, SortOrder.Ascending);
                    RemoveFromList(f, serviceSettingsArg);
                    break;
                case ServiceSettings.Biggest:
                    f = await FileSorter.SortBySizeAsync(Files, SortOrder.Descending);
                    RemoveFromList(f, serviceSettingsArg);
                    break;
                case ServiceSettings.OlderThan:
                    f = await FileSorter.SortByDateAsync(Files, SortOrder.Ascending);
                    DateTime date = DateTime.Now.AddDays(-serviceSettingsArg);
                    RemoveFromList(f, date);
                    break;
                case ServiceSettings.BiggerThan:
                    f = await FileSorter.SortBySizeAsync(Files, SortOrder.Descending);
                    RemoveFromList(f, (double)serviceSettingsArg);
                    break;
                default:
                    break;
            }
        }

        private async void RemoveFromList(IEnumerable<MyFile> f, int files)
        {
            double sizeDeleted;
            List<MyFile> toDelete = new List<MyFile>();
            for (int i = 0; i < files; i++)
            {
                if(i < f.Count())
                {
                    toDelete.Add(f.ElementAt(i));
                    Files.Remove(f.ElementAt(i));
                }
                else
                {
                    break;
                }
            }
            sizeDeleted = await FilesDeleter.DelteFilesAndDirectoriesAsync(toDelete);
            TotalSize -= sizeDeleted / 1024.0;
            if(mySwitch.TraceWarning)
            {
                eventLog1.WriteEntry($"{DateTime.Now} - Deleted Size: {sizeDeleted} KB");
            }
            
        }

        private async void RemoveFromList(IEnumerable<MyFile> f, DateTime date)
        {
            double sizeDeleted;
            List<MyFile> toDelete = new List<MyFile>();
            foreach (var file in f)
            {
                if (file.DateModified < date)
                {
                    toDelete.Add(file);
                    Files.Remove(file);
                }
                else
                {
                    break;
                }
            }
            sizeDeleted = await FilesDeleter.DelteFilesAndDirectoriesAsync(toDelete);
            TotalSize -= sizeDeleted / 1024.0;
            if (mySwitch.TraceWarning)
            {
                eventLog1.WriteEntry($"{DateTime.Now} - Deleted Size: {sizeDeleted} KB");
            }
        }

        private async void RemoveFromList(IEnumerable<MyFile> f, double size)
        {
            double sizeDeleted;
            List<MyFile> toDelete = new List<MyFile>();
            foreach (var file in f)
            {
                if (file.Size > size)
                {
                    toDelete.Add(file);
                    Files.Remove(file);
                }
                else
                {
                    break;
                }
            }
            sizeDeleted = await FilesDeleter.DelteFilesAndDirectoriesAsync(toDelete);
            TotalSize -= sizeDeleted / 1024.0;
            if (mySwitch.TraceWarning)
            {
                eventLog1.WriteEntry($"{DateTime.Now} - Deleted Size: {sizeDeleted} KB");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            NameValueCollection valueCollection = ConfigurationManager.AppSettings;
            DateTime schedule = DateTime.Parse(valueCollection.Get("Schedule"));

            if(schedule.Date == DateTime.Now.Date)
            {
                if (mySwitch.TraceWarning)
                {
                    eventLog1.WriteEntry($"{DateTime.Now} - Regular Cleaning. Cleaning intervals: {mainSettingsArg}");
                }
                ConfigManager.UpdateSchedule(mainSettingsArg);
                HandleSettings();
            }

            /*if(schedule.Minute == DateTime.Now.Minute)
            {
                ConfigManager.UpdateSchedule(mainSettingsArg);
                HandleSettings();
            }*/
            
        }

        protected override void OnStop()
        {
        }
    }
}
