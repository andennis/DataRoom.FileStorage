using Common.Configuration;
using FileStorage.Core;

namespace FileStorage.BL
{
    public sealed class FileStorageConfig : AppConfigBase, IFileStorageConfig
    {
        public FileStorageConfig()
            : base("FileStorage")
        {
        }

        public int StorageDeep
        {
            get
            {
                if (int.TryParse(GetValue<string>("StorageDeep"), out int n))
                    return n;

                return -1;
            }
        }
        public int MaxItemsNumber
        {
            get
            {
                if (int.TryParse(GetValue<string>("MaxItemsNumber"), out int n))
                    return n;

                return -1;
            }
        }
        public string StoragePath => GetValue<string>("StoragePath");
    }
}
