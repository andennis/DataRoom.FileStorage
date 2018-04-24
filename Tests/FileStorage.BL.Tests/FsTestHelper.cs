using System.IO;
using FileStorage.Core;
using FileStorage.Repository.Core;
using FileStorage.Repository.EF;

namespace FileStorage.BL.Tests
{
    public static class FsTestHelper
    {
        public static void ClearFileStorage(IFileStorageConfig fsConfig)
        {
            ClearFileStorageFolder(fsConfig);
            ClearFileStorageDb(fsConfig);
        }

        private static void ClearFileStorageFolder(IFileStorageConfig fsConfig)
        {
            if (!Directory.Exists(fsConfig.StoragePath))
                return;

            string[] dirs = Directory.GetDirectories(fsConfig.StoragePath);
            foreach (string dir in dirs)
                Directory.Delete(dir, true);

            string[] files = Directory.GetFiles(fsConfig.StoragePath);
            foreach (string file in files)
                File.Delete(file);
        }
        private static void ClearFileStorageDb(IFileStorageConfig fsConfig)
        {
            using (IFileStorageUnitOfWork unitOfWork = new FileStorageUnitOfWork(fsConfig))
            {
                unitOfWork.FileStorageRepository.ClearFileStorage();
            }
        }

    }
}
