using System.IO;
using FileStorage.Core;
using FileStorage.Repository.Core;
using FileStorage.Repository.EF;
using Microsoft.Extensions.Configuration;

namespace FileStorage.BL.Tests
{
    public static class FsBlTestHelper
    {
        public static FileStorageConfig GetAppConfig()
        {
            var configBuilder = new ConfigurationBuilder();
            IConfiguration config = configBuilder.AddJsonFile("appsettings.json", true).Build();
            return new FileStorageConfig(config);
        }

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
