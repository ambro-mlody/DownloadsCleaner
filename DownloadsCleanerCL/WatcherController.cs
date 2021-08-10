using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsCleanerCL
{
    public static class WatcherController
    {
        public static FileSystemWatcher InitWatcher(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);

            watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Size |
                NotifyFilters.Security;

            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        private static IEnumerable<MyFile> AddFile(IEnumerable<MyFile> files, string path)
        {
            if (File.Exists(path))
            {
                MyFile file = new MyFile(new FileInfo(path));
                files = files.Append(file);
            }
            else if (Directory.Exists(path))
            {
                MyFile file = new MyFile(new DirectoryInfo(path));
                files = files.Append(file);
            }
            return files;
        }

        public static IEnumerable<MyFile> FileListChanged(FileSystemEventArgs e, IEnumerable<MyFile> FileList)
        {
            IEnumerable<MyFile> files = FileList.Where(f => f.Name == e.Name);
            FileList = DeleteFromList.RemoveFromListWithoutDeleteing(FileList, files);
            return AddFile(FileList, e.FullPath);
        }

        public static IEnumerable<MyFile> FileListRenamed(RenamedEventArgs e, IEnumerable<MyFile> FileList)
        {
            IEnumerable<MyFile> files = FileList.Where(f => f.Name == e.OldName);
            FileList = DeleteFromList.RemoveFromListWithoutDeleteing(FileList, files);
            return AddFile(FileList, e.FullPath);
        }
        
        public static IEnumerable<MyFile> FileListDeleted(FileSystemEventArgs e, IEnumerable<MyFile> FileList)
        {
            IEnumerable<MyFile> files = FileList.Where(f => f.Name == e.Name);
            FileList = DeleteFromList.RemoveFromListWithoutDeleteing(FileList, files);
            return FileList;
        }
        
        public static IEnumerable<MyFile> FileListCreated(FileSystemEventArgs e, IEnumerable<MyFile> FileList)
        {
            return AddFile(FileList, e.FullPath);
        }
    }
}
