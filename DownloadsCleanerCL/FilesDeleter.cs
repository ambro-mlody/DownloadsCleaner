using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsCleanerCL
{
    public static class FilesDeleter
    {
        public static async Task<double> DelteFilesAndDirectoriesAsync(IEnumerable<MyFile> files)
        {
            double sizeDeleted = 0;
            foreach (MyFile file in files)
            {
                sizeDeleted += file.Size;
                if (file.File)
                {
                    File.Delete(file.Path);
                }
                else
                {
                    Directory.Delete(file.Path, true);
                }
            }
            return sizeDeleted;
        }
    }
}
