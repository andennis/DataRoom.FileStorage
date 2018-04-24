using Common.Repository;
using Common.Unity;
using FileStorage.Core;
using Unity;
using Unity.Resolution;

namespace FileStorage.Factory
{
    public static class FileStorageFactory
    {
        private static IUnityContainer _iocContainer;
        
        public static IFileStorageService Create(IFileStorageConfig config = null)
        {
            if (_iocContainer == null)
            {
                _iocContainer = new UnityContainer();
                _iocContainer.LoadUnityConfiguration(config, "FileStorage");
            }

            if (config == null)
                return _iocContainer.Resolve<IFileStorageService>();

            return _iocContainer.Resolve<IFileStorageService>(new DependencyOverride<IFileStorageConfig>(config), 
                new DependencyOverride<IDbConfig>(config));
        }
    }
}
