using DownloadsCleanerCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DownloadsCleanerTestsLibrary
{
    public class WatcherControllerTests
    {

        [Fact]
        public void FileListChanged_FileExists()
        {
            FileSystemEventArgs fileName = new FileSystemEventArgs(WatcherChangeTypes.Changed, @"C:\Users\Tomek\Desktop\temp", "y.txt");
            List<MyFile> files = new List<MyFile>
            {
                new MyFile {Name = "y.txt"}
            };

            files = WatcherController.FileListChanged(fileName, files).ToList();

            Assert.Single(files);
        }

        [Fact]
        public void FileListChanged_FileDontExists()
        {
            FileSystemEventArgs fileName = new FileSystemEventArgs(WatcherChangeTypes.Changed, @"", "y.txt");
            List<MyFile> files = new List<MyFile>
            {
                new MyFile {Name = "y.txt"}
            };

            files = WatcherController.FileListChanged(fileName, files).ToList();

            Assert.Empty(files);
        }
        
        [Fact]
        public void FileListRenamed_FileExists()
        {
            RenamedEventArgs fileName = new RenamedEventArgs(WatcherChangeTypes.Renamed, @"C:\Users\Tomek\Desktop\temp", "y.txt", "y.txt");
            List<MyFile> files = new List<MyFile>
            {
                new MyFile {Name = "y.txt"}
            };

            files = WatcherController.FileListChanged(fileName, files).ToList();

            Assert.Single(files);
        }
        
        [Fact]
        public void FileListRenamed_FileDontExists()
        {
            RenamedEventArgs fileName = new RenamedEventArgs(WatcherChangeTypes.Renamed, @"", "y.txt", "y.txt");
            List<MyFile> files = new List<MyFile>
            {
                new MyFile {Name = "y.txt"}
            };

            files = WatcherController.FileListChanged(fileName, files).ToList();

            Assert.Empty(files);
        }

        [Fact]
        public void FileListDeleted_FileExists()
        {
            FileSystemEventArgs fileName = new FileSystemEventArgs(WatcherChangeTypes.Deleted, @"C:\Users\Tomek\Desktop\temp", "y.txt");
            List<MyFile> files = new List<MyFile>
            {
                new MyFile {Name = "y.txt"}
            };

            files = WatcherController.FileListDeleted(fileName, files).ToList();

            Assert.Empty(files);
        }

        [Fact]
        public void FileListCreated_FileExists()
        {
            FileSystemEventArgs fileName = new FileSystemEventArgs(WatcherChangeTypes.Created, @"C:\Users\Tomek\Desktop\temp", "y.txt");
            List<MyFile> files = new List<MyFile>
            {
            };

            files = WatcherController.FileListCreated(fileName, files).ToList();

            Assert.Single(files);
        }
    }
}
