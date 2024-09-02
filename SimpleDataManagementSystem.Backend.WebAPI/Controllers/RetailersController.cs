using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Services;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;
using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailersController : ControllerBase
    {
        private const string IMAGE_BASE_PATH = "Images\\Retailers";

        private readonly IRetailersService _retailersService;
        //private readonly IFilesService _filesService; // TODO


        public RetailersController(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        [HttpPost]
        public async Task<IActionResult> AddNewRetailer([FromForm] NewRetailerWebApiModel newRetailerWebApiModel)
        {
            string imageUrlPath = null;

            if (newRetailerWebApiModel.LogoImage != null)
            {
                imageUrlPath = Path.Combine(IMAGE_BASE_PATH, Guid.NewGuid() + "_" + newRetailerWebApiModel.LogoImage.FileName);
            }

            int newRetailerId = await _retailersService.AddNewRetailerAsync(new NewRetailerDTO()
            {
                Name = newRetailerWebApiModel.Name,
                Priority = newRetailerWebApiModel.Priority,
                LogoImageUrl = imageUrlPath
            });

            if (newRetailerWebApiModel.LogoImage != null)
            {
                FilesService.Upload(imageUrlPath, newRetailerWebApiModel.LogoImage.OpenReadStream());

                // demo only; "D:\repos\SimpleDataManagementSystem\SimpleDataManagementSystem.Backend.WebAPI\bin\Debug\net7.0"
                //string assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                //using (var fileStream = System.IO.File.Create(Path.Combine(assDirPath, imageUrlPath)))
                //{
                //    newRetailerWebApiModel.LogoImage.CopyTo(fileStream);
                //}
            }

            return Created($"api/retailers/{newRetailerId}", newRetailerId); // TODO return "NewlyCreatedRetailerDTO"
        }

        [HttpDelete("{retailerId}")]
        public async Task<IActionResult> DeleteRetailer([FromRoute] int retailerId)
        {
            var retailer = await _retailersService.GetRetailerByIdAsync(retailerId);

            await _retailersService.DeleteRetailerAsync(retailerId);

            if ((retailer.LogoImageUrl != null) || !(string.IsNullOrEmpty(retailer.LogoImageUrl)))
            {
                FilesService.Delete(retailer.LogoImageUrl);
            }

            return Ok();
        }

        [HttpPut("{retailerId}")]
        public async Task<IActionResult> UpdateRetailer([FromRoute] int retailerId, [FromForm] UpdateRetailerWebApiModel updateRetailerWebApiModel)
        {
            // check if null, delete image & null DB path
            string imageUrlPath = string.Empty;

            if (updateRetailerWebApiModel.LogoImage != null)
            {
                imageUrlPath = Path.Combine(IMAGE_BASE_PATH, Guid.NewGuid() + "_" + updateRetailerWebApiModel.LogoImage.FileName);
            }

            await _retailersService.UpdateRetailerAsync(retailerId, new UpdateRetailerDTO()
            {
                Name = updateRetailerWebApiModel.Name,
                Priority = updateRetailerWebApiModel.Priority,
                LogoImageUrl = imageUrlPath
            });

            if (updateRetailerWebApiModel.LogoImage != null)
            {
                // save image
                FilesService.Upload(imageUrlPath, updateRetailerWebApiModel.LogoImage.OpenReadStream());
            }

            return Ok();
        }

        [HttpGet("{retailerId}")]
        public async Task<IActionResult> GetRetailerById([FromRoute] int retailerId)
        {
            var retailer = await _retailersService.GetRetailerByIdAsync(retailerId);

            var model = new RetailerWebApiModel()
            {
                ID = retailer.ID,
                Name = retailer.Name,
                Priority = retailer.Priority,
                LogoImageUri = Path.Combine(Paths.FILES_BASE_URL, retailer.LogoImageUrl)
            };

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRetailers([FromQuery] int? take = 8, [FromQuery] int? page = 1)
        {
            var retailers = await _retailersService.GetAllRetailersAsync(take, page);

            var models = new List<RetailerWebApiModel>();

            if (retailers != null)
            {
                foreach (var retailer in retailers)
                {
                    models.Add(new RetailerWebApiModel()
                    {
                        ID = retailer.ID,
                        Name = retailer.Name,
                        Priority = retailer.Priority,
                        LogoImageUri = Path.Combine(Paths.FILES_BASE_URL, retailer.LogoImageUrl)
                    });
                }
            }

            return Ok(models);
        }
    }
}
