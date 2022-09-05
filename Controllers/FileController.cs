using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication44Udemy.Entities;
using WebApplication44Udemy.Models;
using WebApplication44Udemy.Services;
using NLog.Web;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace WebApplication44Udemy.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration =1200,VaryByQueryKeys =new string[] { "fileName"})]
        public ActionResult GetFile ([FromQuery] string fileName)
        {
            var root = Directory.GetCurrentDirectory();
            var filePath = $"{root}/privateFiles/{fileName}";

            if(!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out string contentType);
            var file = System.IO.File.ReadAllBytes(filePath);

            return File(file,contentType,fileName);

        }
        [HttpPost]
        public ActionResult Upload([FromForm]IFormFile fileNew)
        {
            if( fileNew!=null && fileNew.Length>0)
            {
                var root = Directory.GetCurrentDirectory();
                var filePath = $"{root}/privateFiles/{fileNew.FileName}";
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileNew.CopyTo(stream);
                }
                return Ok();
            }
            return BadRequest();

                
        }
    }
}
