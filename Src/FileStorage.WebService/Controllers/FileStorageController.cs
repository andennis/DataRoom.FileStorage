using System.IO;
using Common.BL;
using Common.Utils;
using FileStorage.Core;
using FileStorage.Core.Entities;
using FileStorage.WebService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.WebService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FileStorageController : Controller
    {
        private readonly IFileStorageService _fileStorageService;

        public FileStorageController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        public UploadResponse Upload(IFormFile file)
        {
            if (file == null)
                return new UploadResponse(null){Status = ServiceResponseStatus.Error, Message = "File was not passed"};

            Stream strm = file.OpenReadStream();
            string token = _fileStorageService.Put(strm, file.FileName);
            return new UploadResponse(token);
        }

        [HttpGet]
        public IActionResult Download(string token)
        {
            StorageFileInfo sfi = _fileStorageService.GetFile(token, true);
            if (sfi == null)
                return new NotFoundResult();

            string ct = FileHelper.GetContentType(sfi.Name);
            return File(sfi.FileStream, ct ?? "text/plain", sfi.Name);
        }


    }
}
