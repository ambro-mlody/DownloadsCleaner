using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;

namespace DownloadsCleanerCL
{
    public class MyFile
    {
        public ImageSource IconDisp { get; set; }
        public string Name { get; set; }
        public DateTime DateModified { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public bool File { get; set; }

        public MyFile()
        {

        }

        public MyFile(FileInfo file)
        {
            Name = file.Name;
            DateModified = file.CreationTime;
            Size = file.Length;
            Icon icon = Icon.ExtractAssociatedIcon(file.FullName);
            IconDisp = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
            Path = file.FullName;
            File = true;
        }

        public MyFile(DirectoryInfo dir)
        {
            Name = dir.Name;
            DateModified = dir.CreationTime;
            IconDisp = FolderIcon.GetIcon();
            Size = GetDirSize(dir);
            Path = dir.FullName;
            File = false;
        }

        private long GetDirSize(DirectoryInfo dir)
        {
            long size = 0;
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                size += file.Length;
            }
            var dirs = dir.GetDirectories();
            foreach (var d in dirs)
            {
                size += GetDirSize(d);
            }

            return size;
        }
    }
}
