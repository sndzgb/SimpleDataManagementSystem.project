using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Services;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Data;
using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailersController : ControllerBase
    {
        private const string IMAGE_BASE_PATH = "Images\\Retailers";

        private readonly IRetailersService _retailersService;
        private readonly IAuthorizationService _authorizationService;

        // TODO
        // private readonly IFilesService _filesService; 

        // TODO move file upload/ delete to "FilesService" in service layer


        public RetailersController(IRetailersService retailersService, IAuthorizationService authorizationService)
        {
            _retailersService = retailersService;
            _authorizationService = authorizationService;
        }


        [HttpPost]
        public async Task<IActionResult> AddNewRetailer([FromForm] NewRetailerWebApiModel newRetailerWebApiModel)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            string? imageUrlPath = null;

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
            }

            // TODO get newly created retailer from DB, and return it to the client
            return Created($"api/retailers/{newRetailerId}", newRetailerId); 
        }

        [HttpDelete("{retailerId}")]
        public async Task<IActionResult> DeleteRetailer([FromRoute] int retailerId)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

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
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

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
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var retailer = await _retailersService.GetRetailerByIdAsync(retailerId);

            if (retailer == null)
            {
                return NotFound(new ErrorWebApiModel(StatusCodes.Status404NotFound, "The requested resource was not found.", null));
            }

            var model = new RetailerWebApiModel()
            {
                ID = retailer.ID,
                Name = retailer.Name,
                Priority = retailer.Priority,
                LogoImageUri = string.IsNullOrEmpty(retailer.LogoImageUrl) ? null : Path.Combine(Paths.FILES_BASE_URL, retailer.LogoImageUrl)
                //LogoImageUri = Path.Combine(Paths.FILES_BASE_URL, retailer.LogoImageUrl)
            };

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRetailers([FromQuery] int? take = 8, [FromQuery] int? page = 1)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee, (int)Roles.User };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var retailers = await _retailersService.GetAllRetailersAsync(take, page);

            var model = new RetailersWebApiModel();

            model.PageInfo.Total = retailers.PageInfo.Total;
            model.PageInfo.Take = retailers.PageInfo.Take;
            model.PageInfo.Page = retailers.PageInfo.Page;

            if (retailers.Retailers != null)
            {
                foreach (var retailer in retailers.Retailers)
                {
                    model.Retailers.Add(new RetailerWebApiModel()
                    {
                        ID = retailer.ID,
                        Name = retailer.Name,
                        Priority = retailer.Priority,
                        LogoImageUri = string.IsNullOrEmpty(retailer.LogoImageUrl) ? null : Path.Combine(Paths.FILES_BASE_URL, retailer.LogoImageUrl)
                    });
                }
            }

            return Ok(model);
        }

        [HttpPatch("{retailerId}")]
        public async Task<IActionResult> PatchRetailer([FromRoute] int retailerId)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var retailer = await _retailersService.GetRetailerByIdAsync(retailerId);

            if (retailer == null)
            {
                return BadRequest($"Retailer with ID '{retailerId}' was not found");
            }

            await _retailersService.UpdateRetailerPartialAsync(retailer.ID);

            FilesService.Delete(retailer.LogoImageUrl);

            return Ok();
        }
    }
}
