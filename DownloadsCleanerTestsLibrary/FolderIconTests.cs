using DownloadsCleanerCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xunit;

namespace DownloadsCleanerTestsLibrary
{
    public class FolderIconTests
    {
        [Fact]
        public void GetIcon_NotNull()
        {
            ImageSource result = FolderIcon.GetIcon();
            Assert.NotNull(result);
        }
    }
}
