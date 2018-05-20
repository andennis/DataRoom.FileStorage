using System;
using System.IO;
using Common.BL;
using FileStorage.Core;
using FileStorage.Core.Entities;
using FileStorage.WebService.Controllers;
using FileStorage.WebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FileStorage.WebService.Tests
{
    [TestFixture]
    public class FileStorageWebServiceTests
    {
        [Test]
        public void Upload_Success_Test()
        {
            const string token = "112233";
            const string fileName = "file1,txt";

            var fss = new Mock<IFileStorageService>();
            var file = new Mock<IFormFile>();

            file.Setup(x => x.OpenReadStream()).Returns(It.IsAny<Stream>());
            file.Setup(x => x.FileName).Returns(fileName);
            fss.Setup(x => x.Put(It.IsAny<Stream>(), It.IsAny<string>())).Returns(token);

            var fsws = new FileStorageController(fss.Object);
            UploadResponse resp = fsws.Upload(file.Object);
            Assert.IsNotNull(resp);
            Assert.AreEqual(ServiceResponseStatus.Success, resp.Status);
            Assert.AreEqual(token, resp.ItemToken);
            Assert.IsNull(resp.Message);

            file.Verify(x => x.OpenReadStream(), Times.Once);
            fss.Verify(x => x.Put(It.IsAny<Stream>(), It.Is<string>(p => p == fileName)), Times.Once);

        }

        [Test]
        public void Upload_FormFile_is_Null_Test()
        {
            var fss = new Mock<IFileStorageService>();
            var fsws = new FileStorageController(fss.Object);
            UploadResponse resp = fsws.Upload(null);
            Assert.IsNotNull(resp);
            Assert.AreEqual(ServiceResponseStatus.Error, resp.Status);
            Assert.IsNotNull(resp.Message);
        }

        [Test]
        public void Download_Success_Test()
        {
            const string fileName = "file1.txt";
            const string token = "123321";
            var fss = new Mock<IFileStorageService>();
            var fsws = new FileStorageController(fss.Object);

            var sfi = new StorageFileInfo() { Id = 1, Name = fileName, Size = 100, FileStream = new MemoryStream()};
            fss.Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<bool>())).Returns(sfi);

            IActionResult ar = fsws.Download(token);
            Assert.IsNotNull(ar);
            Assert.IsInstanceOf<FileStreamResult>(ar);
            var result = (FileStreamResult) ar;
            Assert.AreEqual("text/plain", result.ContentType);
            Assert.AreEqual(fileName, result.FileDownloadName);
            Assert.IsNotNull(result.FileStream);

            fss.Verify(x => x.GetFile(It.Is<string>(p => p == token), It.Is<bool>(p => p)), Times.Once);
        }

        [Test]
        public void Download_File_Not_Found_Test()
        {
            const string token = "123321";
            var fss = new Mock<IFileStorageService>();
            var fsws = new FileStorageController(fss.Object);

            fss.Setup(x => x.GetFile(It.IsAny<string>(), It.IsAny<bool>())).Returns((StorageFileInfo)null);

            IActionResult ar = fsws.Download(token);
            Assert.IsNotNull(ar);
            Assert.IsInstanceOf<NotFoundResult>(ar);

            fss.Verify(x => x.GetFile(It.Is<string>(p => p == token), It.Is<bool>(p => p)), Times.Once);
        }

    }
}
