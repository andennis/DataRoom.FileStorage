using Common.Repository;
using FileStorage.Repository.Core.Entities;

namespace FileStorage.Repository.Core
{
    public interface IFileStorageRepository : IRepository<FolderItem>
    {
        FolderItem GetFreeFolderItem(int folderLevel, int maxItemsNumber);
        string GetFolderItemPath(long folderItemId);
        string GetStorageItemPath(long storageItemId);
        StorageItem GetStorageItem(long storageItemId);
        void ClearFileStorage();
    }
}
