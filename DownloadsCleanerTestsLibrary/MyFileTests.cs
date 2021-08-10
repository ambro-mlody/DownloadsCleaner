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
    public class MyFileTests
    {
        [Fact]
        public void MyFile_CreatesFromFileInfo()
        {
            FileStream stream = File.Create("TestFile.txt");

            stream.Close();

            FileInfo file = new FileInfo("TestFile.txt");

            MyFile result = new MyFile(file);

            Assert.Equal(result.Name, file.Name);
            Assert.Equal(result.Path, file.FullName);
            Assert.Equal(result.DateModified, file.CreationTime);
            Assert.Equal(result.Size, file.Length);
            Assert.True(result.File);

            File.Delete("TestFile.txt");
        }
    }
}
