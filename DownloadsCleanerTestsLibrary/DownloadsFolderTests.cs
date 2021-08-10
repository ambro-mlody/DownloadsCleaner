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
    public class DownloadsFolderTests
    {
        [Fact]
        public void GetPath_ReturnsGoodPath()
        {
            string expected = @"C:\Users\Tomek\Downloads";

            string actual = DownloadsFolder.GetPath();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TotalSize_ReturnsAccurateSize()
        {
            List<MyFile> files = new List<MyFile>
            {
                new MyFile {Size = 12.5},
                new MyFile {Size = 0},
                new MyFile {Size = 20}
            };

            double expected = 32.5 / 1024.0;
            double actual = DownloadsFolder.TotalSize(files);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetFiles_ExistingDirectory()
        {
            string path = @"C:\Users\Tomek\Desktop\temp";
            Directory.CreateDirectory($@"{path}\Dir");
            FileStream fs = File.Create($@"{path}\Dir\file.txt");
            fs.Close();

            List<MyFile> files = DownloadsFolder.GetFiles($@"{path}\Dir").ToList();
            Assert.Single(files);
            Assert.Equal("file.txt", files[0].Name);

            Directory.Delete($@"{path}\Dir", true);
        }

        [Fact]
        public void GetFiles_NonExistentDirectory()
        {
            List<MyFile> files = DownloadsFolder.GetFiles("Dir").ToList();
            Assert.Empty(files);
        }
    }
}
