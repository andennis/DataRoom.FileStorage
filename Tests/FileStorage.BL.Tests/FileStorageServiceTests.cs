using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileStorage.Core;
using FileStorage.Core.Entities;
using NUnit.Framework;

namespace FileStorage.BL.Tests
{
    [TestFixture]
    public class FileStorageServiceTests
    {
        private const string TestFolderBase = "Data";
        private const string TestFilePath = @"Data\TextFile1.txt";
        private const string TestFolderPath = @"Data\F1";

        private readonly IFileStorageConfig _fsConfig;

        public FileStorageServiceTests()
        {
            _fsConfig = new FileStorageConfig();
        }

        [SetUp]
        public void InitEachTest()
        {
            FsTestHelper.ClearFileStorage(_fsConfig);
        }

        [Test]
        public void FillStorageWithFilesTest()
        {
            using (var fsService = GetFileStorageService())
            {
                for (int i = 0; i < 16; i++)
                    Assert.Greater(fsService.Put(TestFilePath), 0);

                Assert.Throws<FileStorageException>(() => fsService.Put(TestFilePath));
                int count = GetFileCount(_fsConfig.StoragePath);
                Assert.AreEqual(16, count);
            }
        }

        [Test]
        public void FillStorageWithFoldersTest()
        {
            using (var fsService = GetFileStorageService())
            {
                for (int i = 0; i < 16; i++)
                    Assert.Greater(fsService.Put(TestFolderPath), 0);

                Assert.Throws<FileStorageException>(() => fsService.Put(TestFolderPath));
                int count = GetFolderCount(_fsConfig.StoragePath);
                Assert.AreEqual(30, count);
            }
        }

        [Test]
        public void FillStorageWithFoldersAndFilesTest()
        {
            using (var fsService = GetFileStorageService())
            {
                for (int i = 0; i < 8; i++)
                {
                    Assert.Greater(fsService.Put(TestFolderPath), 0); 
                    Assert.Greater(fsService.Put(TestFilePath), 0);
                }

                Assert.Throws<FileStorageException>(() => fsService.Put(TestFolderPath));
                Assert.Throws<FileStorageException>(() => fsService.Put(TestFilePath));

                int count = GetFolderCount(_fsConfig.StoragePath);
                Assert.AreEqual(22, count);
                count = GetFileCount(_fsConfig.StoragePath);
                Assert.AreEqual(16, count);
            }
        }

        [Test]
        public void PutFolderWithCopyTest()
        {
            string srcPath = Path.Combine(TestFolderBase, "F2");
            if (!Directory.Exists(srcPath))
                Directory.CreateDirectory(srcPath);

            string srcFilePath = Path.Combine(srcPath, "TextFile1.txt");
            File.Copy(TestFilePath, srcFilePath, true);

            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(srcPath);
                string dstPath = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(dstPath));
                Assert.True(Directory.Exists(dstPath));
                string dstFilePath = Path.Combine(dstPath, "TextFile1.txt");
                Assert.True(File.Exists(dstFilePath));

                Assert.True(Directory.Exists(srcPath));
                Assert.True(File.Exists(dstFilePath));
            }
        }

        [Test]
        public void PutFolderWithMoveTest()
        {
            string srcPath = Path.Combine(TestFolderBase, "F2");
            if (!Directory.Exists(srcPath))
                Directory.CreateDirectory(srcPath+"\\F3");

            string srcFilePath = Path.Combine(srcPath, "TextFile1.txt");
            File.Copy(TestFilePath, srcFilePath, true);

            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(srcPath, true);
                string dstPath = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(dstPath));
                Assert.True(Directory.Exists(dstPath));
                string dstFilePath = Path.Combine(dstPath, "TextFile1.txt");
                Assert.True(File.Exists(dstFilePath));

                Assert.False(Directory.Exists(srcPath));
                Assert.True(File.Exists(dstFilePath));
            }
        }

        [Test]
        public void PutFileWithCopyTest()
        {
            string srcFilePath = Path.Combine(TestFolderBase, "TextFile2.txt");
            File.Copy(TestFilePath, srcFilePath, true);

            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(srcFilePath);
                string dstFilePath = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(dstFilePath));
                Assert.True(File.Exists(dstFilePath));
                Assert.True(File.Exists(srcFilePath));
            }
        }

        [Test]
        public void PutFileWithMoveTest()
        {
            string srcFilePath = Path.Combine(TestFolderBase, "TextFile2.txt");
            File.Copy(TestFilePath, srcFilePath, true);

            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(srcFilePath, true);
                string dstFilePath = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(dstFilePath));
                Assert.True(File.Exists(dstFilePath));
                Assert.False(File.Exists(srcFilePath));
            }
        }

        [Test]
        public void PutFileByStreamTest()
        {
            using (Stream fs = new FileStream(TestFilePath, FileMode.Open))
            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(fs);
                Assert.Greater(id, 0);
                string dstFilePath = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(dstFilePath));
                Assert.True(File.Exists(dstFilePath));

                fs.Position = 0;
                id = fsService.Put(fs, "File1.txt");
                Assert.Greater(id, 0);
                StorageFileInfo sfi = fsService.GetFile(id);
                Assert.AreEqual("File1.txt", sfi.Name);
            }
        }

        [Test]
        public void CreateStorageFolderTest()
        {
            using (var fsService = GetFileStorageService())
            {
                string folderPath1;
                long id = fsService.CreateStorageFolder(out folderPath1);
                Assert.Greater(id, 0);
                Assert.False(string.IsNullOrEmpty(folderPath1));
                string folderPath2 = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(folderPath2));
                Assert.AreEqual(folderPath1, folderPath2);
                Assert.True(Directory.Exists(folderPath1));
            }
        }

        [Test]
        public void PutToStorageFolderWithCopyTest()
        {
            using (var fsService = GetFileStorageService())
            {
                long id = fsService.CreateStorageFolder(out string folderPath1);
                Assert.Greater(id, 0);

                //Copy file
                fsService.PutToStorageFolder(id, TestFilePath, false);
                string path = Path.Combine(folderPath1, Path.GetFileName(TestFilePath));
                Assert.True(File.Exists(path));

                fsService.PutToStorageFolder(id, TestFilePath, "F1", false);
                path = Path.Combine(folderPath1, "F1", Path.GetFileName(TestFilePath));
                Assert.True(File.Exists(path));

                //Copy folder
                fsService.PutToStorageFolder(id, TestFolderPath, null, false);
                path = Path.Combine(folderPath1, Path.GetFileName(TestFolderPath));
                Assert.True(Directory.Exists(path));
                path = Path.Combine(path, "TextFile2.txt");
                Assert.True(File.Exists(path));

                fsService.PutToStorageFolder(id, TestFolderPath, "F2");
                path = Path.Combine(folderPath1, "F2", Path.GetFileName(TestFolderPath));
                Assert.True(Directory.Exists(path));
                path = Path.Combine(path, "TextFile2.txt");
                Assert.True(File.Exists(path));
            }
        }

        [Test]
        public void PutToStorageFolderWithMoveTest()
        {
            string srcPath = Path.Combine(TestFolderBase, "F2");
            Directory.CreateDirectory(srcPath);

            using (var fsService = GetFileStorageService())
            {
                long id = fsService.CreateStorageFolder(out string folderPath1);
                Assert.Greater(id, 0);

                //Move file
                string srcFilePath1 = Path.Combine(srcPath, "TextFile1.txt");
                File.Copy(TestFilePath, srcFilePath1, true);
                fsService.PutToStorageFolder(id, srcFilePath1, true);
                string path = Path.Combine(folderPath1, Path.GetFileName(srcFilePath1));
                Assert.True(File.Exists(path));
                Assert.False(File.Exists(srcFilePath1));

                File.Copy(TestFilePath, srcFilePath1, true);
                fsService.PutToStorageFolder(id, srcFilePath1, "F1", true);
                path = Path.Combine(folderPath1, "F1", Path.GetFileName(srcFilePath1));
                Assert.True(File.Exists(path));
                Assert.False(File.Exists(srcFilePath1));

                //Move folder
                srcPath = Path.Combine(TestFolderBase, "F3");
                Directory.CreateDirectory(srcPath);
                string srcFilePath2 = Path.Combine(srcPath, "TextFile1.txt");
                File.Copy(TestFilePath, srcFilePath2, true);

                fsService.PutToStorageFolder(id, srcPath, null, true);
                path = Path.Combine(folderPath1, Path.GetFileName(srcPath));
                Assert.True(Directory.Exists(path));
                path = Path.Combine(path, "TextFile1.txt");
                Assert.True(File.Exists(path));

                Directory.CreateDirectory(srcPath);
                srcFilePath2 = Path.Combine(srcPath, "TextFile1.txt");
                File.Copy(TestFilePath, srcFilePath2, true);

                fsService.PutToStorageFolder(id, srcPath, "F2", true);
                path = Path.Combine(folderPath1, "F2", Path.GetFileName(srcPath));
                Assert.True(Directory.Exists(path));
                path = Path.Combine(path, "TextFile1.txt");
                Assert.True(File.Exists(path));
            }
        }

        [Test]
        public void ClearStorageFolderTest()
        {
            using (var fsService = GetFileStorageService())
            {
                //Create storage folder
                long id = fsService.Put(TestFolderBase);
                string strorageFolderPath = fsService.GetStorageItemPath(id);

                CollectionAssert.IsNotEmpty(Directory.GetFiles(strorageFolderPath));
                CollectionAssert.IsNotEmpty(Directory.GetDirectories(strorageFolderPath)); 

                //Clear storage folder
                Assert.DoesNotThrow(() => fsService.ClearStorageFolder(id));
                Assert.True(Directory.Exists(strorageFolderPath));

                CollectionAssert.IsEmpty(Directory.GetFiles(strorageFolderPath));
                CollectionAssert.IsEmpty(Directory.GetDirectories(strorageFolderPath)); 
            }
        }

        [Test]
        public void GetFilePathTest()
        {
            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(TestFilePath);
                string path = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(path));
                Assert.True(path.StartsWith(_fsConfig.StoragePath));
                Assert.True(File.Exists(path));

                id = fsService.Put(TestFolderPath);
                path = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(path));
                Assert.True(path.StartsWith(_fsConfig.StoragePath));
                Assert.True(Directory.Exists(path));
            }
        }

        [Test]
        public void GetFileTest()
        {
            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(TestFilePath);
                StorageFileInfo fileInfo = fsService.GetFile(id);
                Assert.NotNull(fileInfo);
                Assert.False(string.IsNullOrEmpty(fileInfo.Name));
                Assert.AreEqual(Path.GetFileName(TestFilePath), fileInfo.Name);
                Assert.Null(fileInfo.FileStream);

                using (fileInfo = fsService.GetFile(id, true))
                {
                    Assert.NotNull(fileInfo);
                    Assert.NotNull(fileInfo.FileStream);
                }
            }
        }

        [Test]
        public void DeleteStorageItemTest()
        {
            using (var fsService = GetFileStorageService())
            {
                long id = fsService.Put(TestFilePath);
                string path = fsService.GetStorageItemPath(id);
                Assert.False(string.IsNullOrEmpty(path));
                Assert.True(File.Exists(path));

                fsService.DeleteStorageItem(id);
                Assert.IsNull(fsService.GetStorageItemPath(id));
                Assert.True(File.Exists(path));

                Assert.DoesNotThrow(() => fsService.DeleteStorageItem(id));
            }
        }

        [Ignore("It should be implemented later")]
        public void PurgeDeletedItemsTests()
        {
            throw new NotImplementedException();
        }

        private IFileStorageService GetFileStorageService()
        {
            return Factory.FileStorageFactory.Create(_fsConfig);
        }

        private int GetFolderCount(string path)
        {
            IEnumerable<string> dirs = Directory.GetDirectories(path).Select(Path.GetFileName);
            return dirs.Count() + dirs.Sum(dir => GetFolderCount(Path.Combine(path, dir)));
        }
        private int GetFileCount(string path)
        {
            string[] files = Directory.GetFiles(path);
            IEnumerable<string> dirs = Directory.GetDirectories(path).Select(Path.GetFileName);
            return files.Length + dirs.Sum(dir => GetFileCount(Path.Combine(path, dir)));
        }
    }
}

