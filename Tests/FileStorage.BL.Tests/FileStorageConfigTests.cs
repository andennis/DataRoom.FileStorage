using Common.Repository;
using FileStorage.Core;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace FileStorage.BL.Tests
{
    [TestFixture]
    public class FileStorageConfigTests
    {
        [Test]
        public void FileStorageConfigPropertiesTest()
        {
            FileStorageConfig fsConfig = FsBlTestHelper.GetAppConfig();
            Assert.AreEqual(3, fsConfig.StorageDeep);
            Assert.AreEqual(2, fsConfig.MaxItemsNumber);
            Assert.AreEqual("1234567890asdfgh", fsConfig.SecurityKey);
            Assert.False(string.IsNullOrEmpty(fsConfig.StoragePath));
            Assert.False(string.IsNullOrEmpty(fsConfig.ConnectionString));
        }

        [Test]
        public void FileStorageConfigInterfacesTest()
        {
            FileStorageConfig fsConfig = FsBlTestHelper.GetAppConfig();
            Assert.IsInstanceOf<IFileStorageConfig>(fsConfig);
            Assert.IsInstanceOf<IDbConfig>(fsConfig);
        }

    }
}
