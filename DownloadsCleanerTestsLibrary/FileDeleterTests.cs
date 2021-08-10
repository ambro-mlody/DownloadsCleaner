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
    public class FileDeleterTests
    {
        [Fact]
        public void DelteFilesAndDirectoriesAsync_IsDeleteingFile()
        {
            FileStream stream = File.Create("TestFile.txt");

            stream.Close();

            FileInfo file = new FileInfo("TestFile.txt");

            List<MyFile> files = new List<MyFile>()
            {
                new MyFile(file)
            };

            double expectedSizeDeleted = file.Length;
            double sizeDeleted = FilesDeleter.DelteFilesAndDirectoriesAsync(files).Result;

            bool isDeleted = !File.Exists("TestFile.txt");

            Assert.Equal(expectedSizeDeleted, sizeDeleted);
            Assert.True(isDeleted);
        }

        [Fact]
        public void DelteFilesAndDirectoriesAsync_IsDeleteingDirectory()
        {
            DirectoryInfo dir = Directory.CreateDirectory("TestDirectory");

            MyFile directory = new MyFile(dir);

            List<MyFile> dirs = new List<MyFile>()
            {
                directory
            };

            double expectedSizeDeleted = directory.Size;
            double sizeDeleted = FilesDeleter.DelteFilesAndDirectoriesAsync(dirs).Result;

            bool isDeleted = !Directory.Exists("TestFile.txt");

            Assert.Equal(expectedSizeDeleted, sizeDeleted);
            Assert.True(isDeleted);
        }
    }
}
