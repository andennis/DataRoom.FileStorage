using FileStorage.Repository.Core;
using FileStorage.Repository.Core.Entities;
using NUnit.Framework;

namespace FileStorage.Repository.EF.Tests
{
    [TestFixture]
    public class FileStorageUnitOfWorkTests
    {
        [Test]
        public void FileStorageRepositoryTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                Assert.NotNull(unitOfWork.FileStorageRepository);
                Assert.IsInstanceOf<IFileStorageRepository>(unitOfWork.FileStorageRepository);
                Assert.IsInstanceOf<IFileStorageRepository>(unitOfWork.GetRepository<FolderItem>());
            }
        }

        [Test]
        public void GetRepositoryTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                Assert.IsNotNull(unitOfWork.GetRepository<StorageItem>());
                Assert.IsNotNull(unitOfWork.GetRepository<FolderItem>());
            }
        }

        private IFileStorageUnitOfWork GetFileStorageUnitOfWork()
        {
            return new FileStorageUnitOfWork(FsRepTestHelper.DbConfig);
        }
    }
}
