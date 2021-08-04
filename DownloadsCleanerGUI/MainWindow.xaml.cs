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

namespace DownloadsCleanerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public MainWindow()
        {
            GetFiles();
            InitializeComponent();
        }

        private void GetFiles()
        {
            downloadsInfo = new DirectoryInfo(path);
            var dirs = downloadsInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                Files.Add(new MyFile(dir));
            }
            var files1 = downloadsInfo.GetFiles();
            foreach (var file in files1)
            {
                Files.Add(new MyFile(file));
            }
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
                
                Files.Remove(item);
            }

        }
    }
}
