using Common.Repository;
using Common.Repository.EF;
using FileStorage.Repository.Core;

namespace FileStorage.Repository.EF
{
    public sealed class FileStorageUnitOfWork : UnitOfWork, IFileStorageUnitOfWork
    {
        private IFileStorageRepository _fileStorageRepository;

        public FileStorageUnitOfWork(IDbConfig dbConfig)
            :base(new FileStorageDbContext(dbConfig.ConnectionString))
        {
            RegisterCustomRepository(() => FileStorageRepository);
        }

        public IFileStorageRepository FileStorageRepository => _fileStorageRepository ?? (_fileStorageRepository = new FileStorageRepository(_dbContext));
    }
}
