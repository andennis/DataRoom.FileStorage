using Common.Repository;
using FileStorage.Core;
using NUnit.Framework;

namespace FileStorage.BL.Tests
{
    [TestFixture]
    public class FileStorageConfigTests
    {
        [Test]
        public void FileStorageConfigPropertiesTest()
        {
            var fsConfig = new FileStorageConfig();
            Assert.AreEqual(3, fsConfig.StorageDeep);
            Assert.AreEqual(2, fsConfig.MaxItemsNumber);
            Assert.AreEqual("1234567890asdfgh", fsConfig.SecurityKey);
            Assert.False(string.IsNullOrEmpty(fsConfig.StoragePath));
            Assert.False(string.IsNullOrEmpty(fsConfig.ConnectionString));
        }

        [Test]
        public void FileStorageConfigInterfacesTest()
        {
            var fsConfig = new FileStorageConfig();
            Assert.IsInstanceOf<IFileStorageConfig>(fsConfig);
            Assert.IsInstanceOf<IDbConfig>(fsConfig);
        }
    }
}
