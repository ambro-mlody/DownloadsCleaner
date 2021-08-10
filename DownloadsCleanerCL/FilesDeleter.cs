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
        public static Task<double> DelteFilesAndDirectoriesAsync(IEnumerable<MyFile> files)
        {
            double sizeDeleted = 0;
            foreach (MyFile file in files)
            {
                sizeDeleted += file.Size;
                if (file.File)
                {
                    if(File.Exists(file.Path))
                    {
                        File.Delete(file.Path);
                    }
                }
                else
                {
                    if(Directory.Exists(file.Path))
                    {
                        Directory.Delete(file.Path, true);
                    }
                }
            }
            return Task.FromResult(sizeDeleted);
        }
    }
}
