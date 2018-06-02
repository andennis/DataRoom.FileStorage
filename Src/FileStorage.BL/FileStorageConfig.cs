using Common.Configuration;
using FileStorage.Core;
using Microsoft.Extensions.Configuration;

namespace FileStorage.BL
{
    public sealed class FileStorageConfig : AppConfigBase, IFileStorageConfig
    {
        public FileStorageConfig(IConfiguration config)
            : base(config, "FileStorage")
        {
        }

        public int StorageDeep
        {
            get
            {
                if (int.TryParse(GetValue<string>(nameof(StorageDeep)), out int n))
                    return n;

                return -1;
            }
        }
        public int MaxItemsNumber
        {
            get
            {
                if (int.TryParse(GetValue<string>(nameof(MaxItemsNumber)), out int n))
                    return n;

                return -1;
            }
        }
        public string StoragePath => GetValue<string>(nameof(StoragePath));
        public string SecurityKey => GetValue<string>(nameof(SecurityKey));
    }
}
