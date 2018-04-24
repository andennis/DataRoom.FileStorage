using System.Data.SqlClient;
using System.Linq;
using Common.Repository.EF;
using FileStorage.Repository.Core;
using FileStorage.Repository.Core.Entities;

namespace FileStorage.Repository.EF
{
    public sealed class FileStorageRepository : Repository<FolderItem>, IFileStorageRepository
    {
        public FileStorageRepository(DbContextBase dbContext)
            : base(dbContext, null)
        {
        }

        public FolderItem GetFreeFolderItem(int folderLevel, int maxItemsNumber)
        {
            return SqlQueryStoredProc(DbScheme + ".GetFreeFolder",
                                          new SqlParameter("FolderLevel", folderLevel),
                                          new SqlParameter("MaxItemsNumber", maxItemsNumber)).FirstOrDefault();
        }

        public string GetFolderItemPath(long folderItemId)
        {
            return SqlQueryScalarStoredProc<string>(DbScheme + ".GetFolderPath",
                                          new SqlParameter("FolderItemId", folderItemId));
        }

        public string GetStorageItemPath(long storageItemId)
        {
            return SqlQueryScalarStoredProc<string>(DbScheme + ".GetStorageItemPath",
                                          new SqlParameter("StorageItemId", storageItemId));
        }

        public StorageItem GetStorageItem(long storageItemId)
        {
            return ((FileStorageDbContext)_dbContext).StorageItems.FirstOrDefault(x => x.StorageItemId == storageItemId && x.Status == ItemStatus.Active);
        }

        public void ClearFileStorage()
        {
            //using (var trn = new TransactionScope())
            {
                ExecuteCommand($"DELETE FROM {DbScheme}.StorageItem");
                ExecuteCommand($"DELETE FROM {DbScheme}.FolderItem");
                //trn.Complete();
            }
        }
    }
}
