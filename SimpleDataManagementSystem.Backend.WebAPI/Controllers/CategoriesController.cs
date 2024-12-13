using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Controllers.Base;
using SimpleDataManagementSystem.Backend.WebAPI.Helpers;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Data;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICategoriesCoreService _categoriesCoreService;


        public CategoriesController(
                IAuthorizationService authorizationService,
                ICategoriesCoreService categoriesCoreService
            )
        {
            _authorizationService = authorizationService;
            _categoriesCoreService = categoriesCoreService;
        }


        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetSingleCategory(int categoryId, CancellationToken cancellationToken)
        {
            var canGetSingleCategory = await CanGetSingleCategoryAsync(HttpContext);

            if (!canGetSingleCategory)
            {
                return Forbid();
            }

            var category = await _categoriesCoreService.GetCategoryAsync(new Logic.DTOs.Request.GetCategoryRequestDTO()
            {
                CategoryId = categoryId,
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            if (category == null)
            {
                return NotFound();
            }

            var response = MapToGetSingleCategoryResponse(category);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMultipleCategories(
                CancellationToken cancellationToken,
                int? take = 8, 
                int? page = 1
            )
        {
            var canGetMultipleCategories = await CanGetMultipleCategoriesAsync(HttpContext);

            if (!canGetMultipleCategories)
            {
                return Forbid();
            }

            var categories = await _categoriesCoreService.GetCategoriesAsync(new Logic.DTOs.Request.GetCategoriesRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                PageInfo = new Logic.DTOs.Request.GetCategoriesRequestDTO.PageDTO()
                {
                    Page = (int)page,
                    Take = (int)take
                }
            }, cancellationToken);

            var response = MapToGetMultipleCategoriesResponse(categories);
            response.PageInfo.Page = (int)page;
            response.PageInfo.Take = (int)take;

            return Ok(response);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(
                [FromRoute] int categoryId,
                UpdateCategoryRequestWebApiModel updateCategoryRequestWebApiModel,
                CancellationToken cancellationToken
            )
        {
            var canUpdateCategory = await CanUpdateCategoryAsync(HttpContext);

            if (!canUpdateCategory)
            {
                return Forbid();
            }

            await _categoriesCoreService.UpdateCategoryAsync(
                new Logic.DTOs.Request.UpdateCategoryRequestDTO(
                        new Logic.DTOs.Request.UpdateCategoryRequestDTO.UpdateCategoryRequestMetadata(categoryId)
                    )
                {
                    Name = updateCategoryRequestWebApiModel.Name,
                    RequestedByUserId = GetUserId(),
                    Priority = updateCategoryRequestWebApiModel.Priority
                }, cancellationToken
            );

            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var canDeleteCategory = await CanDeleteCategoryAsync(HttpContext);

            if (!canDeleteCategory)
            {
                return Forbid();
            }

            await _categoriesCoreService.DeleteCategoryAsync(new Logic.DTOs.Request.DeleteCategoryRequestDTO(
                    new Logic.DTOs.Request.DeleteCategoryRequestDTO.DeleteCategoryRequestMetadata(categoryId)
                )
            {
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(
                [FromBody] CreateCategoryRequestWebApiModel createCategoryRequestWebApiModel,
                CancellationToken cancellationToken
            )
        {
            var canCreateCategory = await CanCreateCategoryAsync(HttpContext);

            if (!canCreateCategory)
            {
                return Forbid();
            }

            var newCategory = await _categoriesCoreService.CreateCategoryAsync(new Logic.DTOs.Request.CreateCategoryRequestDTO() 
            {
                RequestedByUserId = GetUserId(),
                Priority = createCategoryRequestWebApiModel.Priority,
                Name = createCategoryRequestWebApiModel.Name
            }, cancellationToken);

            var response = MapToCreateCategoryResponse(newCategory);

            return Ok(response);
            //return CreatedAtAction(nameof(GetSingleCategory), new { id = response.Id }, response);
        }


        #region Permissions

        private async Task<bool> CanDeleteCategoryAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanCreateCategoryAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanUpdateCategoryAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetMultipleCategoriesAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee, (int)Roles.User };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetSingleCategoryAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee, (int)Roles.User };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        #endregion


        #region Mapping

        private CreateCategoryResponseWebApiModel MapToCreateCategoryResponse(CreateCategoryResponseDTO newCategory)
        {
            var response = new CreateCategoryResponseWebApiModel()
            {
                Priority = newCategory.Priority,
                Id = newCategory.Id,
                Name = newCategory.Name
            };

            return response;
        }

        private GetSingleCategoryResponseWebApiModel MapToGetSingleCategoryResponse(GetCategoryResponseDTO getCategoryResponseDTO)
        {
            var response = new GetSingleCategoryResponseWebApiModel();
            response.Priority = getCategoryResponseDTO.Priority;
            response.Name = getCategoryResponseDTO.Name;
            response.Id = getCategoryResponseDTO.Id;

            return response;
        }

        private GetMultipleCategoriesResponseWebApiModel? MapToGetMultipleCategoriesResponse(GetCategoriesResponseDTO getCategoriesResponseDTO)
        {
            var response = new GetMultipleCategoriesResponseWebApiModel();

            if (getCategoriesResponseDTO == null)
            {
                return null;
            }

            response.Categories = new List<GetMultipleCategoriesResponseWebApiModel.CategoryWebApiModel>();
            response.Categories.AddRange(
                getCategoriesResponseDTO.Categories.Select(x => new GetMultipleCategoriesResponseWebApiModel.CategoryWebApiModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Priority = x.Priority
                })
            );

            response.PageInfo = new GetMultipleCategoriesResponseWebApiModel.PageWebApiModel()
            {
                Total = getCategoriesResponseDTO.PageInfo.Total
            };

            return response;
        }

        #endregion
    }
}
