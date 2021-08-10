using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadsCleanerCL
{
    public static class DeleteFromList
    {
        public static async Task<IEnumerable<MyFile>> RemoveFromCorrespondingListAsync(IEnumerable<MyFile> RemoveList, IEnumerable<MyFile> SourceList)
        {
            _ = await FilesDeleter.DelteFilesAndDirectoriesAsync(SourceList);
            return RemoveFromListWithoutDeleteing(RemoveList, SourceList);
        }

        public static IEnumerable<MyFile> RemoveFromListWithoutDeleteing(IEnumerable<MyFile> RemoveList, IEnumerable<MyFile> SourceList)
        {
            IQueryable<MyFile> rl = RemoveList.AsQueryable();
            IQueryable<MyFile> sl = SourceList.AsQueryable();
            IEnumerable<MyFile> result = rl.Except(sl);
            return result;
        }

        public static async Task<IEnumerable<MyFile>> RemoveFromListAsync(IEnumerable<MyFile> RemoveList, int NumberOfFiles)
        {
            List<MyFile> toDelete = new List<MyFile>();
            for (int i = 0; i < NumberOfFiles && i < RemoveList.Count(); i++)
            {
                toDelete.Add(RemoveList.ElementAt(i));
                
            }
            return await RemoveFromCorrespondingListAsync(RemoveList, toDelete);
        }

        public static async Task<IEnumerable<MyFile>> RemoveFromListAsync(IEnumerable<MyFile> RemoveList, DateTime OlderThan)
        {
            List<MyFile> toDelete = new List<MyFile>();
            foreach (MyFile file in RemoveList)
            {
                if(file.DateModified < OlderThan)
                {
                    toDelete.Add(file);
                }
                else
                {
                    break;
                }
            }
            return await RemoveFromCorrespondingListAsync(RemoveList, toDelete);
        }

        public static async Task<IEnumerable<MyFile>> RemoveFromListAsync(IEnumerable<MyFile> RemoveList, double BiggerThan)
        {
            List<MyFile> toDelete = new List<MyFile>();
            foreach (MyFile file in RemoveList)
            {
                if (file.Size > BiggerThan)
                {
                    toDelete.Add(file);
                }
                else
                {
                    break;
                }
            }
            return await RemoveFromCorrespondingListAsync(RemoveList, toDelete);
        }
    }
}
