using System;
using System.Collections.ObjectModel;
using System.Linq;
using FileStorage.Repository.Core;
using FileStorage.Repository.Core.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace FileStorage.Repository.EF.Tests
{
    [TestFixture]
    public class FileStorageRepositoryTests
    {
        [SetUp]
        public void InitEachTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                unitOfWork.FileStorageRepository.ClearFileStorage();
            }
        }

        [Test]
        public void FolderItemCrudOperationsTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                var fiRep = unitOfWork.GetRepository<FolderItem>();
                Assert.NotNull(fiRep);

                var fi1 = new FolderItem() {Name = "FI1"};
                Assert.DoesNotThrow(() => fiRep.Insert(fi1));
                Assert.DoesNotThrow(unitOfWork.Save);
                Assert.Greater(fi1.FolderItemId, 0);

                FolderItem copyFi1 = null;
                Assert.DoesNotThrow(() => copyFi1 = fiRep.Find(fi1.FolderItemId));
                Assert.AreEqual(fi1.Name, copyFi1.Name);

                fi1.Name = "FI1_New";
                Assert.DoesNotThrow(() => fiRep.Update(fi1));
                Assert.DoesNotThrow(unitOfWork.Save);
                copyFi1 = fiRep.Find(fi1.FolderItemId);
                Assert.NotNull(copyFi1);
                Assert.AreEqual(fi1.Name, copyFi1.Name);

                Assert.DoesNotThrow(() => fiRep.Delete(fi1));
                Assert.DoesNotThrow(unitOfWork.Save);
                copyFi1 = fiRep.Find(fi1.FolderItemId);
                Assert.Null(copyFi1);

                Assert.DoesNotThrow(() => fiRep.Delete(fi1));
                Assert.Catch<Exception>(unitOfWork.Save);
                copyFi1 = fiRep.Find(fi1.FolderItemId);
                Assert.NotNull(copyFi1);
                copyFi1 = fiRep.Query().Filter(x => x.FolderItemId == fi1.FolderItemId).Get().FirstOrDefault();
                Assert.Null(copyFi1);
            }
        }

        [Test]
        public void FolderItemChildObjectsCrudOperationsTest()
        {
            FolderItem fi1, fi2;
            StorageItem si1;
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                var fiRep = unitOfWork.GetRepository<FolderItem>();
                Assert.NotNull(fiRep);

                fi1 = new FolderItem() {Name = "FI1"};
                Assert.DoesNotThrow(() => fiRep.Insert(fi1));
                Assert.DoesNotThrow(unitOfWork.Save);
                Assert.Greater(fi1.FolderItemId, 0);
                
                fi2 = new FolderItem() {Name = "FI2"};
                fi1.ChildFolders = new Collection<FolderItem>() {fi2};
                si1 = new StorageItem() {Name = "SI1"};
                fi1.ChildStorageItems = new Collection<StorageItem>() {si1};
                Assert.DoesNotThrow(() => fiRep.Update(fi1));
                Assert.DoesNotThrow(unitOfWork.Save);

                FolderItem copyFi2 = fiRep.Find(fi2.FolderItemId);
                Assert.NotNull(copyFi2);
                Assert.AreEqual(fi2.Name, copyFi2.Name);
                Assert.NotNull(copyFi2.Parent);
                Assert.AreEqual(fi1.Name, copyFi2.Parent.Name);
            }

            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                var fiRep = unitOfWork.GetRepository<FolderItem>();
                FolderItem copyFi1 = fiRep.Query()
                    .Include(x => x.ChildStorageItems)
                    .Include(x => x.ChildFolders)
                    .Filter(x => x.FolderItemId == fi1.FolderItemId)
                    .Get().FirstOrDefault();

                Assert.NotNull(copyFi1);
                Assert.NotNull(copyFi1.ChildStorageItems);
                Assert.NotNull(copyFi1.ChildFolders);
                copyFi1.ChildStorageItems.Should().HaveCount(1);
                copyFi1.ChildFolders.Should().HaveCount(1);
                copyFi1.ChildStorageItems.Should().Contain(x => x.StorageItemId == si1.StorageItemId);
                copyFi1.ChildFolders.Should().Contain(x => x.FolderItemId == fi2.FolderItemId);
            }
        }

        [Test]
        public void StorageItemCrudOperationsTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                var fiRep = unitOfWork.GetRepository<FolderItem>();
                var siRep = unitOfWork.GetRepository<StorageItem>();

                //Create parent folder item
                var fi = new FolderItem() {Name = "FI1"};
                Assert.DoesNotThrow(() => fiRep.Insert(fi));
                Assert.DoesNotThrow(unitOfWork.Save);
                Assert.Greater(fi.FolderItemId, 0);

                //Create storage item
                var si1 = new StorageItem()
                          {
                              Name = "SI1",
                              Parent = fi,
                              ItemType = StorageItemType.File,
                              Status = ItemStatus.Active
                          };
                Assert.DoesNotThrow(() => siRep.Insert(si1));
                Assert.DoesNotThrow(unitOfWork.Save);
                Assert.Greater(si1.StorageItemId, 0);

                StorageItem copySi1 = null;
                Assert.DoesNotThrow(() => copySi1 = siRep.Find(si1.StorageItemId));
                Assert.AreEqual(si1.Name, copySi1.Name);

                si1.Name = "SI1_New";
                Assert.DoesNotThrow(() => siRep.Update(si1));
                Assert.DoesNotThrow(unitOfWork.Save);
                copySi1 = siRep.Find(si1.StorageItemId);
                Assert.NotNull(copySi1);
                Assert.AreEqual(si1.Name, copySi1.Name);

                Assert.DoesNotThrow(() => siRep.Delete(si1));
                Assert.DoesNotThrow(unitOfWork.Save);
                copySi1 = siRep.Find(si1.StorageItemId);
                Assert.Null(copySi1);

                Assert.DoesNotThrow(() => siRep.Delete(si1));
                Assert.Catch<Exception>(unitOfWork.Save);
                copySi1 = siRep.Find(si1.StorageItemId);
                Assert.NotNull(copySi1);
                copySi1 = siRep.Query().Filter(x => x.StorageItemId == si1.StorageItemId).Get().FirstOrDefault();
                Assert.Null(copySi1);
            }
        }

        [Test]
        public void GetFreeFolderItemTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                Assert.DoesNotThrow(() => unitOfWork.FileStorageRepository.GetFreeFolderItem(3, 3));
            }
        }

        [Test]
        public void GetFolderItemPathTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                Assert.DoesNotThrow(() => unitOfWork.FileStorageRepository.GetFolderItemPath(0));
            }
        }

        [Test]
        public void GetStorageItemPathTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                Assert.DoesNotThrow(() => unitOfWork.FileStorageRepository.GetStorageItemPath(0));
            }
        }

        [Test]
        public void GetStorageItemTest()
        {
            using (var unitOfWork = GetFileStorageUnitOfWork())
            {
                Assert.DoesNotThrow(() => unitOfWork.FileStorageRepository.GetStorageItem(0));
            }
        }

        private IFileStorageUnitOfWork GetFileStorageUnitOfWork()
        {
            return new FileStorageUnitOfWork(TestHelper.DbConfig);
        }
    }
}
