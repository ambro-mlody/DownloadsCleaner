using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace DownloadsCleanerCL
{
    public static class DownloadsFolder
    {
        private static string downloadsGuid = "{374DE290-123F-4565-9164-39C4925E467B}";

        public static string getPath()
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
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)]Guid guid, uint dwFlags, IntPtr htoken, out IntPtr ppszPath);
    }
}
