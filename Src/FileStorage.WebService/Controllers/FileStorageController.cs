using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileStorage.Core;
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
        public long Upload(IFormFile file)
        {
            if (file == null)
                return null;

            Stream strm = file.OpenReadStream();
            return _fileStorageService.Put(strm, file.FileName);
        }

        [HttpGet]
        public void Download(string token)
        {
            
        }


    }
}
