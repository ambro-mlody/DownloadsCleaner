using DownloadsCleanerCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DownloadsCleanerTestsLibrary
{
    public class DeleteFromListTests
    {
        private List<MyFile> ListToRemoveFrom = new List<MyFile>
            {
                new MyFile {Name = "e", Path = "a", DateModified = DateTime.Now.AddMinutes(-2)},
                new MyFile {Name = "b", Path = "a", DateModified = DateTime.Now.AddMinutes(-2)},
                new MyFile {Name = "c", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "f", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "a", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "d", Path = "a", DateModified = DateTime.Now}
            };

        [Fact]
        public void RemoveFromListWithoutDeleteing_RemovesFromList()
        {
            List<MyFile> CorrespondingList = new List<MyFile>
            {
                new MyFile {Name = "a", Path = "a", DateModified = DateTime.Now.AddMinutes(-2)},
                new MyFile {Name = "b", Path = "a", DateModified = DateTime.Now.AddMinutes(-2)},
                new MyFile {Name = "c", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "d", Path = "a", DateModified = DateTime.Now}
            };

            List<MyFile> expected = new List<MyFile>
            {
                new MyFile {Name = "e", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "f", Path = "a", DateModified = DateTime.Now}
            };

            List<MyFile> actual = DeleteFromList.RemoveFromListWithoutDeleteing(ListToRemoveFrom, CorrespondingList).ToList();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveFromList_NumberOfFiles()
        {
            int filesToDelete = 2;
            List<MyFile> result = DeleteFromList.RemoveFromListAsync(ListToRemoveFrom, filesToDelete).Result.ToList();

            int expected = 4;
            int actual = result.Count;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RemoveFromList_OlderThan()
        {
            DateTime olderThan = DateTime.Now.AddMinutes(-1);


            List<MyFile> actual = DeleteFromList.RemoveFromListAsync(ListToRemoveFrom, olderThan).Result.ToList();
            List<MyFile> expected = new List<MyFile>
            {
                new MyFile {Name = "c", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "f", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "a", Path = "a", DateModified = DateTime.Now},
                new MyFile {Name = "d", Path = "a", DateModified = DateTime.Now}
            };

            Assert.Equal(expected, actual);
        }
    }
}
