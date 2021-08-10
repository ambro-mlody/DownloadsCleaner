using DownloadsCleanerCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DownloadsCleanerTestsLibrary
{
    public class FileSorterTests
    {

        private List<MyFile> files = new List<MyFile>();

        private readonly MyFile file1 = new MyFile() { Name = "a", Size = 5, DateModified = DateTime.Now.AddHours(1) };
        private readonly MyFile file2 = new MyFile() { Name = "b", Size = 2, DateModified = DateTime.Now };

        public FileSorterTests()
        {
            files.Add(file1);
            files.Add(file2);
        }

        [Fact]
        public void SortByName_IsSortingAscending()
        {
            List<MyFile> actualOrder = FileSorter.SortByNameAsync(files, SortOrder.Ascending).Result.ToList();

            List<MyFile> expectedOrder = new List<MyFile>()
            {
                file1,
                file2
            };

            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public void SortByName_IsSortingDescending()
        {
            List<MyFile> actualOrder = FileSorter.SortByNameAsync(files, SortOrder.Descending).Result.ToList();

            List<MyFile> expectedOrder = new List<MyFile>()
            {
                file2,
                file1
            };

            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public void SortBySize_IsSortingAscending()
        {
            List<MyFile> actualOrder = FileSorter.SortBySizeAsync(files, SortOrder.Ascending).Result.ToList();

            List<MyFile> expectedOrder = new List<MyFile>()
            {
                file2,
                file1
            };

            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public void SortBySize_IsSortingDescending()
        {
            List<MyFile> actualOrder = FileSorter.SortBySizeAsync(files, SortOrder.Descending).Result.ToList();

            List<MyFile> expectedOrder = new List<MyFile>()
            {
                file1,
                file2
            };

            Assert.Equal(expectedOrder, actualOrder);
        }
        [Fact]
        public void SortByDate_IsSortingAscending()
        {
            List<MyFile> actualOrder = FileSorter.SortByDateAsync(files, SortOrder.Ascending).Result.ToList();

            List<MyFile> expectedOrder = new List<MyFile>()
            {
                file2,
                file1
            };

            Assert.Equal(expectedOrder, actualOrder);
        }

        [Fact]
        public void SortByDate_IsSortingDescending()
        {
            List<MyFile> actualOrder = FileSorter.SortByDateAsync(files, SortOrder.Descending).Result.ToList();

            List<MyFile> expectedOrder = new List<MyFile>()
            {
                file1,
                file2
            };

            Assert.Equal(expectedOrder, actualOrder);
        }
    }
}
