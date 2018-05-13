using System;
using System.IO;
using FileStorage.Core.Entities;

namespace FileStorage.Core
{
    public interface IFileStorageService : IDisposable
    {
        /// <summary>
        /// Put file or directory content into storage
        /// </summary>
        /// <param name="fileOrDirPath"></param>
        /// <param name="move"></param>
        /// <returns>Storage item ID</returns>
        string Put(string fileOrDirPath, bool move = false);
        string Put(Stream fileStream, string fileName = null);
        StorageFileInfo GetFile(string itemToken, bool fileStream = false);

        string GetStorageItemPath(string itemToken);

        /// <summary>
        /// Create storage item as folder
        /// </summary>
        /// <param name="folderPath">The absolute path to the storage item (folder)</param>
        /// <returns>Storage item ID</returns>
        string CreateStorageFolder(out string folderPath);

        void PutToStorageFolder(string itemToken, string srcFileOrDirPath, bool move = false);

        /// <summary>
        /// Put file or directory content into specified directory of storage item(folder)
        /// </summary>
        /// <param name="itemToken">Storage item ID</param>
        /// <param name="srcFileOrDirPath">File or directory path</param>
        /// <param name="dstDirPath">directory path in scope of storage item(folder)</param>
        /// <param name="move">Move files and directories if True or copy if False</param>
        void PutToStorageFolder(string itemToken, string srcFileOrDirPath, string dstDirPath, bool move = false);
        void ClearStorageFolder(string itemToken);

        void DeleteStorageItem(string itemToken);
        void PurgeDeletedItems();
    }
}
