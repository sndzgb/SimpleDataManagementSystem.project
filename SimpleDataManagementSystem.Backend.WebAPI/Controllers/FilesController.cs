using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // TODO
        //private readonly IFilesService _filesService; 

        public FilesController()
        {
            
        }


        [HttpGet("images/{category}/{image}")]
        [AllowAnonymous]
        public IActionResult GetImage(string category, string image) 
        {
            var extension = Path.GetExtension(image);

            string assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var path = Path.Combine(assDirPath, "Images", category, image);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            return File(System.IO.File.ReadAllBytes(path), "image/jpeg"); // TODO png, jpg, jpeg - get request file extension
        }
    }
}
