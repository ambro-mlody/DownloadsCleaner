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

    public static class Sorter
    {
        public static ObservableCollection<MyFile> SortByName(ObservableCollection<MyFile> files, SortOrder order)
        {
            var f = files as List<MyFile>;
            switch (order)
            {
                case SortOrder.Ascending:
                    f.Sort();
                    break;
                case SortOrder.Descending:
                    f.Sort();
                    f.Reverse();
                    break;
                default:
                    break;
            }
            files = f;
        }
    }
}
