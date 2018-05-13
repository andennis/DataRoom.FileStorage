using Common.Configuration.Unity;
using Common.Repository;

namespace FileStorage.Core
{
    public interface IFileStorageConfig : IDbConfig, IUnityConfiguration
    {
        int StorageDeep { get; }
        int MaxItemsNumber { get; }
        string StoragePath { get; }
        string SecurityKey { get; }
    }
}