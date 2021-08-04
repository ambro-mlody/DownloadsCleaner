using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsCleanerCL
{
    public enum SortOrder
    {
        Ascending = 0,
        Descending = 1
    }

    public static class FileSorter
    {
        public static async Task<ObservableCollection<MyFile>> SortByName(ObservableCollection<MyFile> files, SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    files = new ObservableCollection<MyFile>(files.OrderBy(f => f.Name));
                    break;
                case SortOrder.Descending:
                    files = new ObservableCollection<MyFile>(files.OrderByDescending(f => f.Name));
                    break;
                default:
                    break;
            }
            return files;
        }
        
        public static async Task<ObservableCollection<MyFile>> SortBySize(ObservableCollection<MyFile> files, SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    files = new ObservableCollection<MyFile>(files.OrderBy(f => f.Size));
                    break;
                case SortOrder.Descending:
                    files = new ObservableCollection<MyFile>(files.OrderByDescending(f => f.Size));
                    break;
                default:
                    break;
            }
            return files;
        }
        
        public static async Task<ObservableCollection<MyFile>> SortByDate(ObservableCollection<MyFile> files, SortOrder order)
        {
            switch (order)
            {
                case SortOrder.Ascending:
                    files = new ObservableCollection<MyFile>(files.OrderBy(f => f.DateModified));
                    break;
                case SortOrder.Descending:
                    files = new ObservableCollection<MyFile>(files.OrderByDescending(f => f.DateModified));
                    break;
                default:
                    break;
            }
            return files;
        }
    }
}
