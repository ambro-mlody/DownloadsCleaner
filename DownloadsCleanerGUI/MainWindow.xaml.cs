using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using DownloadsCleanerCL;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceProcess;

namespace DownloadsCleanerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double totalSize;

        public double TotalSize
        {
            get { return totalSize; }
            set 
            { 
                totalSize = value;
                OnPropertyChanged();
            }
        }

        private int filesCount;

        public int FilesCount
        {
            get { return filesCount; }
            set 
            { 
                filesCount = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<MyFile> files = new ObservableCollection<MyFile>();
        public ObservableCollection<MyFile> Files 
        { 
            get { return files; }
            set
            {
                files = value;
                OnPropertyChanged();
            }
        }
        private string path = DownloadsFolder.getPath();
        private DirectoryInfo downloadsInfo;
        private SortOrder order = SortOrder.Ascending;

        private ServiceController serviceController;

        public MainWindow()
        {
            TotalSize = 0;
            GetFiles();
            InitializeComponent();
            GetSettings();
            InitServiceController();
        }

        private void InitServiceController()
        {
            ServiceController[] serviceControllers;
            serviceControllers = ServiceController.GetServices();
            foreach (var sc in serviceControllers)
            {
                if(sc.ServiceName == "DownloadsCleanerService")
                {
                    serviceController = new ServiceController("DownloadsCleanerService");
                    return;
                }
            }
        }

        private void GetFiles()
        {
            downloadsInfo = new DirectoryInfo(path);
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
            FilesCount = Files.Count;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async void FileNameColumn_Click(object sender, RoutedEventArgs e)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    Files = await FileSorter.SortByName(Files, SortOrder.Ascending);
                    order = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    Files = await FileSorter.SortByName(Files, SortOrder.Descending);
                    order = SortOrder.Ascending;
                    break;
                default:
                    break;
            }
            
        }

        private async void DateColumn_Click(object sender, RoutedEventArgs e)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    Files = await FileSorter.SortByDate(Files, SortOrder.Ascending);
                    order = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    Files = await FileSorter.SortByDate(Files, SortOrder.Descending);
                    order = SortOrder.Ascending;
                    break;
                default:
                    break;
            }
        }

        private async void SizeColumn_Click(object sender, RoutedEventArgs e)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    Files = await FileSorter.SortBySize(Files, SortOrder.Ascending);
                    order = SortOrder.Descending;
                    break;
                case SortOrder.Descending:
                    Files = await FileSorter.SortBySize(Files, SortOrder.Descending);
                    order = SortOrder.Ascending;
                    break;
                default:
                    break;
            }
        }

        private async void DeleteFileMI_Click(object sender, RoutedEventArgs e)
        {
            
            await DeleteFiles();
        }

        private async Task DeleteFiles()
        {
            var selected = FileListView.SelectedItems.Cast<MyFile>().ToArray();
            foreach (var item in selected)
            {
                if(item.File)
                {
                    File.Delete(item.Path);
                }
                else
                {
                    Directory.Delete(item.Path, true);
                }
                TotalSize -= item.Size / 1024.0;
                Files.Remove(item);
            }

        }

        private async void AdvancedDelete1Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (OldestRB.IsChecked.HasValue)
            {
                if (OldestRB.IsChecked.Value)
                {
                    var f = await FileSorter.SortByDate(files, SortOrder.Ascending);
                    RemoveFromList(f);
                    return;
                }
            }

            if (BiggestRB.IsChecked.HasValue)
            {
                if (BiggestRB.IsChecked.Value)
                {
                    var f = await FileSorter.SortBySize(files, SortOrder.Descending);
                    RemoveFromList(f);
                }
            }
        }

        private async void RemoveFromList(ObservableCollection<MyFile> f)
        {
            double sizeDeleted;
            List<MyFile> toDelete = new List<MyFile>();
            for (int i = 0; i < FilesNumberSB.Value.Value; i++)
            {
                toDelete.Add(f.ElementAt(i));
                Files.Remove(f.ElementAt(i));
            }
            sizeDeleted = await FilesDeleter.DelteFilesAndDirectoriesAsync(toDelete);
            TotalSize -= sizeDeleted / 1024.0;
        }

        private async void RemoveFromList(ObservableCollection<MyFile> f, DateTime date)
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
        }

        private async void RemoveFromList(ObservableCollection<MyFile> f, double size)
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
        }

        private void DeleteDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            OlderThanRB.IsChecked = true;
        }

        private async void AdvancedDelete2Button_Click(object sender, RoutedEventArgs e)
        {
            if(OlderThanRB.IsChecked.HasValue)
            {
                if(OlderThanRB.IsChecked.Value)
                {
                    if(DeleteDP.SelectedDate.HasValue)
                    {
                        var f = await FileSorter.SortByDate(files, SortOrder.Ascending);
                        RemoveFromList(f, DeleteDP.SelectedDate.Value);
                        return;
                    }
                }
            }

            if (BiggerThanRB.IsChecked.HasValue)
            {
                if (BiggerThanRB.IsChecked.Value)
                {
                    var f = await FileSorter.SortBySize(files, SortOrder.Descending);
                    RemoveFromList(f, BiggerThanSB.Value.Value);
                    return;
                }
            }
        }

        private void SB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (((sender as Xceed.Wpf.Toolkit.IntegerUpDown).Parent as StackPanel).Parent as RadioButton).IsChecked = true;
        }

        private ConfigManager configManager = new ConfigManager();

        private void GetSettings()
        {
            configManager.GetSettings();
            switch (configManager.MainSettings)
            {
                case ServiceMainSettings.Interval:
                    SetIntervalRB.IsChecked = true;
                    SetIntervalSB.Value = configManager.Days;
                    break;
                case ServiceMainSettings.Limit:
                    SetLimitRB.IsChecked = true;
                    SetLimitSB.Value = configManager.Limit;
                    break;
                default:
                    break;
            }

            switch (configManager.ServiceSettings)
            {
                case ServiceSettings.Oldest:
                    OldestServiceRB.IsChecked = true;
                    OldestServiceSB.Value = configManager.Files;
                    break;
                case ServiceSettings.Biggest:
                    BiggestServiceRB.IsChecked = true;
                    BiggestServiceSB.Value = configManager.Files;
                    break;
                case ServiceSettings.OlderThan:
                    OlderThanServiceRB.IsChecked = true;
                    OlderThanServiceSB.Value = configManager.OlderThan;
                    break;
                case ServiceSettings.BiggerThan:
                    BiggerThanServiceRB.IsChecked = true;
                    BiggerThanServiceSB.Value = configManager.Size;
                    break;
                default:
                    break;
            }
        }

        private async void ConfirmServiceButton_Click(object sender, RoutedEventArgs e)
        {
            string[] args = new string[4];

            if (SetIntervalRB.IsChecked.Value)
            {
                configManager.MainSettings = ServiceMainSettings.Interval;
                configManager.Days = SetIntervalSB.Value.Value;
                args[1] = configManager.Days.ToString();
            }
            else
            {
                configManager.MainSettings = ServiceMainSettings.Limit;
                configManager.Limit = SetLimitSB.Value.Value;
                args[1] = configManager.Limit.ToString();
            }

            if (OldestServiceRB.IsChecked.Value)
            {
                configManager.ServiceSettings = ServiceSettings.Oldest;
                configManager.Files = OldestServiceSB.Value.Value;
                args[3] = configManager.Files.ToString();
            }
            else if (BiggestServiceRB.IsChecked.Value)
            {
                configManager.ServiceSettings = ServiceSettings.Biggest;
                configManager.Files = BiggestServiceSB.Value.Value;
                args[3] = configManager.Files.ToString();
            }
            else if (OlderThanServiceRB.IsChecked.Value)
            {
                configManager.ServiceSettings = ServiceSettings.OlderThan;
                configManager.OlderThan = OlderThanServiceSB.Value.Value;
                args[3] = configManager.OlderThan.ToString();
            }
            else
            {
                configManager.ServiceSettings = ServiceSettings.BiggerThan;
                configManager.Size = BiggerThanServiceSB.Value.Value;
                args[3] = configManager.Size.ToString();
            }

            await configManager.SetSettingsAsync();

            args[0] = configManager.MainSettings.ToString();
            args[2] = configManager.ServiceSettings.ToString();

            if(serviceController.Status == ServiceControllerStatus.Running)
            {
                serviceController.Stop();
                serviceController.Start(args);
            }
        }

        private void StartServiceButton_Click(object sender, RoutedEventArgs e)
        {
            serviceController.Refresh();
            if(serviceController.Status == ServiceControllerStatus.Stopped)
            {
                serviceController.Start();
            }
        }

        private void StopServiceButton_Click(object sender, RoutedEventArgs e)
        {
            serviceController.Refresh();
            if (serviceController.Status == ServiceControllerStatus.Running)
            {
                serviceController.Stop();
            }
        }
    }
}
