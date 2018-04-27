using Common.Repository;

namespace FileStorage.Repository.Core
{
    public interface IFileStorageUnitOfWork : IUnitOfWork
    {
        IFileStorageRepository FileStorageRepository { get; }
    }
}