using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace DownloadsCleanerCL
{
    public static class DownloadsFolder
    {
        private static string downloadsGuid = "{374DE290-123F-4565-9164-39C4925E467B}";

        public static IEnumerable<MyFile> GetFiles(string path)
        {
            List<MyFile> Files = new List<MyFile>();
            if(Directory.Exists(path))
            {
                DirectoryInfo downloadsInfo = new DirectoryInfo(path);
                var files1 = downloadsInfo.GetFiles();
                foreach (var file in files1)
                {
                    Files.Add(new MyFile(file));
                }
                var dirs = downloadsInfo.GetDirectories();
                foreach (var dir in dirs)
                {
                    Files.Add(new MyFile(dir));
                }
            }

            return Files;
        }

        public static double TotalSize(IEnumerable<MyFile> Files)
        {
            double TotalSize = 0;
            foreach (var file in Files)
            {
                TotalSize += file.Size;
            }
            TotalSize /= 1024.0;
            return TotalSize;
        }

        public static string GetPath()
        {
            // 0x00004000 - dont verify
            int result = SHGetKnownFolderPath(new Guid(downloadsGuid), (uint)0x00004000, new IntPtr(0), out IntPtr outPath);
            if(result >= 0)
            {
                string path = Marshal.PtrToStringUni(outPath);
                Marshal.FreeCoTaskMem(outPath);
                return path;
            }
            else
            {
                throw new ExternalException("Unable to retrive downloads folder on this system.", result);
            }

            //return @"C:\Users\Tomek\Desktop\temp";
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]Guid guid, uint dwFlags, IntPtr htoken, out IntPtr ppszPath);
    }
}
