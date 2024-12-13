using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Controllers.Base;
using SimpleDataManagementSystem.Backend.WebAPI.Services;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Data;
using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailersController : BaseController
    {
        private readonly IRetailersCoreService _retailersCoreService;
        private readonly IAuthorizationService _authorizationService;

        private const string RETAILER_IMAGE_BASE_RELATIVE_PATH = "Images\\Retailers";


        public RetailersController(IRetailersCoreService retailersCoreService, IAuthorizationService authorizationService)
        {
            _retailersCoreService = retailersCoreService;
            _authorizationService = authorizationService;
        }


        [HttpGet("{retailerId}")]
        public async Task<IActionResult> GetSingleRetailer([FromRoute] int retailerId, CancellationToken cancellationToken)
        {
            var canGetSingleRetailer = await CanGetSingleRetailerAsync(this.HttpContext);

            if (!canGetSingleRetailer)
            {
                return Forbid();
            }

            var retailer = await _retailersCoreService.GetRetailerAsync(new GetRetailerRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                RetailerId = retailerId
            }, cancellationToken);

            if (retailer == null)
            {
                return NotFound();
            }

            var response = MapToGetSingleRetailerResponse(retailer);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMultipleRetailers(
                CancellationToken cancellationToken,
                [FromQuery] int? take = 8, 
                [FromQuery] int? page = 1
            )
        {
            var canGetMultipleRetailers = await CanGetMultipleRetailersAsync(this.HttpContext);

            if (!canGetMultipleRetailers)
            {
                return Forbid();
            }

            var retailers = await _retailersCoreService.GetRetailersAsync(new GetRetailersRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                PageInfo = new GetRetailersRequestDTO.PageDTO()
                {
                    Take = (int)take,
                    Page = (int)page
                }
            }, cancellationToken);

            var response = MapToGetMultipleRetailersResponse(retailers);
            response.PageInfo.Page = (int)page;
            response.PageInfo.Take = (int)take;

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRetailer([FromRoute] int retailerId, CancellationToken cancellationToken)
        {
            var canDeleteRetailer = await CanDeleteRetailerAsync(this.HttpContext);

            if (!canDeleteRetailer)
            {
                return Forbid();
            }

            // get retailer so we can delete image
            var retailer = await _retailersCoreService.GetRetailerAsync(new GetRetailerRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                RetailerId = retailerId,
            }, cancellationToken);

            if (retailer == null)
            {
                return Ok();
            }

            await _retailersCoreService.DeleteRetailerAsync(new DeleteRetailerRequestDTO(
                    new DeleteRetailerRequestDTO.DeleteRetailerRequestMetadata(retailerId)
                )
            {
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            // delete image
            if ((retailer.LogoImageUrl != null) || !(string.IsNullOrEmpty(retailer.LogoImageUrl)))
            {
                FilesService.Delete(retailer.LogoImageUrl);
            }

            return Ok();
        }

        [HttpPut("{retailerId}")]
        public async Task<IActionResult> UpdateRetailer(
                [FromRoute] int retailerId, 
                [FromForm] UpdateRetailerRequestWebApiModel updateRetailerRequestWebApiModel, 
                CancellationToken cancellationToken
            )
        {
            var canUpdateRetailer = await CanUpdateRetailerAsync(this.HttpContext);

            if (!canUpdateRetailer)
            {
                return Forbid();
            }

            var existingRetailer = await _retailersCoreService.GetRetailerAsync(new GetRetailerRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                RetailerId = retailerId
            }, cancellationToken);

            if (existingRetailer == null)
            {
                return NotFound();
            }

            string? imageUrlPath = null;

            if (updateRetailerRequestWebApiModel.LogoImage != null)
            {
                imageUrlPath = Path.Combine(
                    RETAILER_IMAGE_BASE_RELATIVE_PATH, Guid.NewGuid() + "_" + updateRetailerRequestWebApiModel.LogoImage.FileName
                );
            }

            await _retailersCoreService.UpdateRetailerAsync(new UpdateRetailerRequestDTO(
                        new UpdateRetailerRequestDTO.UpdateRetailerRequestMetadata(retailerId)
                    )
                {
                    LogoImageUrl = imageUrlPath,
                    Name = updateRetailerRequestWebApiModel.Name,
                    Priority = updateRetailerRequestWebApiModel.Priority,
                    RequestedByUserId = GetUserId(),
                    DeleteCurrentLogoImage = updateRetailerRequestWebApiModel.DeleteCurrentLogoImage
                },
                cancellationToken
            );

            // delete image if requested
            if (updateRetailerRequestWebApiModel.DeleteCurrentLogoImage)
            {
                FilesService.Delete(existingRetailer.LogoImageUrl);
            }

            // upload if requested
            if (updateRetailerRequestWebApiModel.LogoImage != null)
            {
                // TODO use Timestamp instead of Guid
                FilesService.Upload(
                    //Path.Combine(
                    //    RETAILER_IMAGE_BASE_RELATIVE_PATH, Guid.NewGuid() + "_" + updateRetailerRequestWebApiModel.LogoImage.FileName
                    //),
                    imageUrlPath,
                    updateRetailerRequestWebApiModel.LogoImage.OpenReadStream()
                );
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRetailer(
                [FromForm] CreateRetailerRequestWebApiModel createRetailerRequestWebApiModel, 
                CancellationToken cancellationToken
            )
        {
            var canCreateRetailer = await CanCreateRetailerAsync(HttpContext);

            if (!canCreateRetailer)
            {
                return Forbid();
            }

            string? imageUrlPath = null;

            if (createRetailerRequestWebApiModel.LogoImage != null)
            {
                imageUrlPath = Path.Combine(
                    RETAILER_IMAGE_BASE_RELATIVE_PATH, Guid.NewGuid() + "_" + createRetailerRequestWebApiModel.LogoImage.FileName
                );
            }

            var createdRetailer = await _retailersCoreService.CreateRetailerAsync(new CreateRetailerRequestDTO()
            {
                LogoImageUrl = imageUrlPath,
                Priority = createRetailerRequestWebApiModel.Priority,
                Name = createRetailerRequestWebApiModel.Name,
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            if (createRetailerRequestWebApiModel.LogoImage != null)
            {
                FilesService.Upload(imageUrlPath, createRetailerRequestWebApiModel.LogoImage.OpenReadStream());
            }

            var response = MapToCreateRetailerResponse(createdRetailer);

            return Ok(response);
            //return CreatedAtAction(nameof(GetSingleRetailer), new { id = response.Id }, response);
        }


        #region Permissions

        private async Task<bool> CanUpdateRetailerAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }
        
        private async Task<bool> CanGetSingleRetailerAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetMultipleRetailersAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanDeleteRetailerAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanCreateRetailerAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        #endregion


        #region Mapping

        private GetSingleRetailerResponseWebApiModel MapToGetSingleRetailerResponse(GetRetailerResponseDTO getRetailerResponseDTO)
        {
            var response = new GetSingleRetailerResponseWebApiModel()
            {
                Id = getRetailerResponseDTO.Id,
                LogoImageUrl = string.IsNullOrEmpty(
                    getRetailerResponseDTO.LogoImageUrl) ? null : Path.Combine(Paths.FILES_BASE_URL, getRetailerResponseDTO.LogoImageUrl),
                Name = getRetailerResponseDTO.Name,
                Priority = getRetailerResponseDTO.Priority
            };

            return response;
        }

        private GetMultipleRetailersResponseWebApiModel MapToGetMultipleRetailersResponse(GetRetailersResponseDTO getRetailersResponseDTO)
        {
            var response = new GetMultipleRetailersResponseWebApiModel();
            response.Retailers = new List<GetMultipleRetailersResponseWebApiModel.RetailerWebApiModel>();
            response.PageInfo = new GetMultipleRetailersResponseWebApiModel.PageWebApiModel()
            {
                Total = getRetailersResponseDTO.PageInfo.Total
            };
            response.Retailers.AddRange(getRetailersResponseDTO.Retailers.Select(x => new GetMultipleRetailersResponseWebApiModel.RetailerWebApiModel()
            {
                Id = x.Id,
                LogoImageUrl = string.IsNullOrEmpty(
                    x.LogoImageUrl) ? null : Path.Combine(Paths.FILES_BASE_URL, x.LogoImageUrl),
                Name = x.Name,
                Priority = x.Priority
            }).ToList());

            return response;
        }

        //private UpdateRetailerRequestDTO? MapToUpdateRetailerRequest(int retailerId, UpdateRetailerRequestWebApiModel model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }

        //    var result = new UpdateRetailerRequestDTO();
        //    result.Metadata = new UpdateRetailerRequestDTO.UpdateRetailerRequestMetadataDTO()
        //    {
        //        RetailerId = retailerId
        //    };
        //    result.Request = new UpdateRetailerRequestDTO.RequestDTO()
        //    {
        //        Name = model.Name,
        //        LogoImageUrl = model.LogoImageUrl,
        //        Priority = model.Priority,
        //        UserId = GetUserId()
        //    };

        //    return result;
        //}

        private CreateRetailerResponseWebApiModel MapToCreateRetailerResponse(CreateRetailerResponseDTO createRetailerResponseDTO)
        {
            var result = new CreateRetailerResponseWebApiModel();
            result.Priority = createRetailerResponseDTO.Priority;
            result.Name = createRetailerResponseDTO.Name;
            result.Id = createRetailerResponseDTO.Id;
            result.LogoImageUrl = string.IsNullOrEmpty(
                    createRetailerResponseDTO.LogoImageUrl) ? null : Path.Combine(Paths.FILES_BASE_URL, createRetailerResponseDTO.LogoImageUrl);

            return result;
        }

        #endregion
    }
}
