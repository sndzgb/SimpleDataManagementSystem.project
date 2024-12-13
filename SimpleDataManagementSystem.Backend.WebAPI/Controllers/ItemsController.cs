using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Controllers.Base;
using SimpleDataManagementSystem.Backend.WebAPI.Helpers;
using SimpleDataManagementSystem.Backend.WebAPI.Hubs;
using SimpleDataManagementSystem.Backend.WebAPI.Services;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Linq;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : BaseController
    {
        private readonly IItemsCoreService _itemsCoreService;
        private readonly IAuthorizationService _authorizationService;

        private const string ITEM_IMAGE_BASE_RELATIVE_PATH = "Images\\Items";


        public ItemsController(IItemsCoreService itemsCoreService, IAuthorizationService authorizationService)
        {
            _itemsCoreService = itemsCoreService;
            _authorizationService = authorizationService;
        }


        [HttpPost("{itemId}/monitored")]
        public async Task<IActionResult> ToggleMonitoredItem(
                [FromRoute] string itemId,
                CancellationToken cancellationToken
            )
        {
            var canToggleMonitoredItem = await CanToggleMonitoredItemAsync(HttpContext);

            if (!canToggleMonitoredItem)
            {
                return Forbid();
            }

            itemId = Uri.UnescapeDataString(itemId.Trim());

            await _itemsCoreService.ToggleMonitoredItemAsync(
                new ToggleMonitoredItemRequestDTO(
                    new ToggleMonitoredItemRequestDTO.ToggleMonitoredItemRequestMetadata(itemId),
                    GetUserId()
                ), cancellationToken
            );

            return Ok();
        }

        [HttpPost("{itemId}/status")]
        public async Task<IActionResult> ToggleItemEnabledDisabledStatus(
                [FromRoute] string itemId, 
                CancellationToken cancellationToken
            )
        {
            //return NotFound(new ErrorWebApiModel(404, "Item not found.", null));

            var canToggleItemEnabledDisabledStatus = await CanToggleItemEnabledDisabledStatusAsync(HttpContext);

            if (!canToggleItemEnabledDisabledStatus)
            {
                return Forbid();
            }

            itemId = Uri.UnescapeDataString(itemId);

            await _itemsCoreService.ToggleItemEnabledDisabledStatusAsync(new ToggleItemEnabledDisabledStatusRequestDTO(
                    new ToggleItemEnabledDisabledStatusRequestDTO.ToggleItemEnabledDisabledStatusRequestMetadata(itemId)
                )
            {
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            return Ok();
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetSingleItem([FromRoute] string itemId, CancellationToken cancellationToken)
        {
            var canGetItem = await CanGetItemAsync(HttpContext);

            if (!canGetItem)
            {
                return Forbid();
            }

            itemId = Uri.UnescapeDataString(itemId);

            var item = await _itemsCoreService.GetItemAsync(new Logic.DTOs.Request.GetItemRequestDTO()
            {
                IncludeCategory = true,
                IncludeRetailer = true,
                Monitoring = new Logic.DTOs.Request.GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = true
                },
                ItemId = itemId,
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            if (item == null)
            {
                return NotFound();
            }

            var response = MapToGetSingleItemResponse(item);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMultipleItems(
                CancellationToken cancellationToken,
                [FromQuery(Name = "enabled_only")] bool? enabledOnly = true,
                [FromQuery] int? take = 8,
                [FromQuery] int? page = 1
            )
        {
            //return NotFound(null);

            var canGetMultipleItems = await CanGetMultipleItemsAsync(HttpContext);

            if (!canGetMultipleItems)
            {
                return Forbid();
            }

            //enabledOnly = false;

            var items = await _itemsCoreService.GetItemsAsync(new Logic.DTOs.Request.GetItemsRequestDTO()
            {
                EnabledOnly = (bool)enabledOnly,
                IncludeCategory = true,
                IncludeRetailer = true,
                Monitoring = new Logic.DTOs.Request.GetItemsRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = true
                },
                PageInfo = new Logic.DTOs.Request.GetItemsRequestDTO.PageDTO()
                {
                    Page = (int)page,
                    Take = (int)take
                },
                RequestedByUserId = GetUserId(),
            }, cancellationToken);

            var response = MapToGetMultipleItemsResponse(items);
            response.PageInfo.Page = (int)page;
            response.PageInfo.Take = (int)take;

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(
                [FromForm] CreateItemRequestWebApiModel createItemRequestWebApiModel, 
                CancellationToken cancellationToken
            )
        {
            //return Forbid();
            //return NotFound(new ErrorWebApiModel(StatusCodes.Status404NotFound, "The requested resource was not found", null));

            var canCreateItem = await CanCreateItemAsync(HttpContext);

            if (!canCreateItem)
            {
                return Forbid();
            }

            string? imageUrlPath = null;

            if (createItemRequestWebApiModel.URLdoslike != null)
            {
                imageUrlPath = Path.Combine(ITEM_IMAGE_BASE_RELATIVE_PATH, Guid.NewGuid() + "_" + createItemRequestWebApiModel.URLdoslike.FileName);
            }

            var createdItem = await _itemsCoreService.CreateItemAsync(new Logic.DTOs.Request.CreateItemRequestDTO()
            {
                RequestedByUserId = GetUserId(),
                URLdoslike = imageUrlPath,
                Opis = createItemRequestWebApiModel.Opis,
                Nazivproizvoda = createItemRequestWebApiModel.Nazivproizvoda,
                IsEnabled = createItemRequestWebApiModel.IsEnabled,
                Datumakcije = createItemRequestWebApiModel.Datumakcije,
                Cijena = createItemRequestWebApiModel.Cijena,
                Kategorija = createItemRequestWebApiModel.Kategorija,
                RetailerId = createItemRequestWebApiModel.RetailerId
            }, cancellationToken);

            if (createItemRequestWebApiModel.URLdoslike != null)
            {
                FilesService.Upload(imageUrlPath, createItemRequestWebApiModel.URLdoslike.OpenReadStream());
            }

            var response = MapToCreateItemResponse(createdItem);

            return CreatedAtAction(nameof(GetSingleItem), new { itemId = response.Nazivproizvoda }, response);
            return Ok(response); // TODO
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] string itemId, CancellationToken cancellationToken)
        {
            var canDeleteItem = await CanDeleteItemAsync(HttpContext);

            if (!canDeleteItem)
            {
                return Forbid();
            }

            itemId = Uri.UnescapeDataString(itemId);

            // get item so we can delete image
            var item = await _itemsCoreService.GetItemAsync(new Logic.DTOs.Request.GetItemRequestDTO()
            {
                IncludeCategory = false,
                IncludeRetailer = false,
                ItemId = itemId,
                Monitoring = new Logic.DTOs.Request.GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = false
                },
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            if (item == null)
            {
                return Ok();
            }

            await _itemsCoreService.DeleteItemAsync(new Logic.DTOs.Request.DeleteItemRequestDTO(
                    new Logic.DTOs.Request.DeleteItemRequestDTO.DeleteItemRequestMetadata(itemId)
                )
            {
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            // delete image
            if ((item.URLdoslike != null) || !(string.IsNullOrEmpty(item.URLdoslike)))
            {
                FilesService.Delete(item.URLdoslike);
            }

            return Ok();
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(
                [FromRoute] string itemId,
                [FromForm] UpdateItemRequestWebApiModel updateItemRequestWebApiModel, 
                CancellationToken cancellationToken
            )
        {
            var canUpdateItem = await CanUpdateItem(HttpContext);

            if (!canUpdateItem) 
            {
                return Forbid();
            }

            itemId = Uri.UnescapeDataString(itemId);

            var existingItem = await _itemsCoreService.GetItemAsync(new Logic.DTOs.Request.GetItemRequestDTO()
            {
                IncludeCategory = true,
                IncludeRetailer = true,
                ItemId = itemId,
                Monitoring = new Logic.DTOs.Request.GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = true
                },
                RequestedByUserId = GetUserId()
            }, cancellationToken);

            if (existingItem == null)
            {
                return NotFound();
            }

            string? imageUrlPath = null;

            if (updateItemRequestWebApiModel.URLdoslike != null)
            {
                imageUrlPath = Path.Combine(
                    ITEM_IMAGE_BASE_RELATIVE_PATH, Guid.NewGuid() + "_" + updateItemRequestWebApiModel.URLdoslike.FileName
                );
            }

            await _itemsCoreService.UpdateItemAsync(
                new Logic.DTOs.Request.UpdateItemRequestDTO(
                        new Logic.DTOs.Request.UpdateItemRequestDTO.UpdateItemRequestMetadata(itemId)
                    )
                {
                    Cijena = updateItemRequestWebApiModel.Cijena,
                    Datumakcije = updateItemRequestWebApiModel.Datumakcije,
                    Opis = updateItemRequestWebApiModel.Opis,
                    RequestedByUserId = GetUserId(),
                    RetailerId = updateItemRequestWebApiModel.RetailerId,
                    IsEnabled = updateItemRequestWebApiModel.IsEnabled,
                    CategoryId = updateItemRequestWebApiModel.Kategorija,
                    IsMonitoredByUser = updateItemRequestWebApiModel.IsMonitoredByUser,
                    DeleteCurrentURLdoslike = updateItemRequestWebApiModel.DeleteCurrentURLdoslike,
                    URLdoslike = imageUrlPath, // set new path, if present
                }, 
                cancellationToken
            );

            // delete image if requested
            if (updateItemRequestWebApiModel.DeleteCurrentURLdoslike)
            {
                FilesService.Delete(existingItem.URLdoslike);
            }

            // upload new if present
            if (updateItemRequestWebApiModel.URLdoslike != null)
            {
                // TODO use Timestamp instead of Guid
                FilesService.Upload(
                    imageUrlPath,
                    updateItemRequestWebApiModel.URLdoslike.OpenReadStream()
                );
            }

            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchItems(
                string? query, 
                string? sortBy, 
                CancellationToken cancellationToken,
                bool? onlyEnabled = true,
                int? page = 1,
                int? take = 8 
            )
        {
            var canSearchItems = await CanSearchItemsAsync(HttpContext);

            if (!canSearchItems)
            {
                return Forbid();
            }

            Logic.DTOs.SortableItem sort = Logic.DTOs.SortableItem.NazivproizvodaAsc;

            if (Int32.TryParse(sortBy, out int num)) // TODO helper GetEnumOrDefault();
            {
                sort = (Logic.DTOs.SortableItem)num;
            }

            var searchResult = await _itemsCoreService.SearchItemsAsync(new Logic.DTOs.Request.SearchItemsRequestDTO()
            {
                IncludeCategory = true,
                IncludeRetailer = true,
                Monitoring = new Logic.DTOs.Request.SearchItemsRequestDTO.ItemMonitoringDTO()
                {
                    IncludeMonitoring = true
                },
                RequestedByUserId = GetUserId(),
                PageInfo = new Logic.DTOs.Request.SearchItemsRequestDTO.PageDTO()
                {
                    IsEnabled = (bool)onlyEnabled,
                    Take = (int)take,
                    Page = (int)page,
                    Query = query,
                    SortBy = sort
                }
            }, cancellationToken);

            var response = MapToSearchItemsResponse(searchResult);
            response.PageInfo.Page = (int)page;
            response.PageInfo.Take = (int)take;

            return Ok(response);
        }


        #region Mapping

        private SearchItemsResponseWebApiModel MapToSearchItemsResponse(SearchItemsResponseDTO searchItemsResponseDTO)
        {
            var responseModel = new SearchItemsResponseWebApiModel();
            responseModel.Items = new List<SearchItemsResponseWebApiModel.ItemWebApiModel>();
            responseModel.Items.AddRange(searchItemsResponseDTO.Items.Select(x => new SearchItemsResponseWebApiModel.ItemWebApiModel()
            {
                Category = new SearchItemsResponseWebApiModel.ItemWebApiModel.CategoryWebApiModel()
                {
                    Id = x.Category.Id
                },
                Retailer = new SearchItemsResponseWebApiModel.ItemWebApiModel.RetailerWebApiModel()
                {
                    Id = x.Retailer.Id
                },
                Cijena = x.Cijena,
                Datumakcije = x.Datumakcije,
                IsEnabled = x.IsEnabled,
                IsMonitoredByCurrentUser = x.IsMonitoredByCurrentUser,
                Nazivproizvoda = x.Nazivproizvoda,
                Opis = x.Opis,
                TotalUsersMonitoringThisItem = x.TotalUsersMonitoringThisItem,
                URLdoslike = string.IsNullOrEmpty(x.URLdoslike) ? null : Path.Combine(Paths.FILES_BASE_URL, x.URLdoslike)
            }).ToList());
            responseModel.PageInfo = new SearchItemsResponseWebApiModel.PageWebApiModel() 
            {
                Total = searchItemsResponseDTO.PageInfo.Total
            };

            return responseModel;
        }

        //private Logic.DTOs.Request.UpdateItemRequestDTO MapToUpdateItemRequest(string nazivproizvoda, UpdateItemRequestWebApiModel model)
        //{
        //    var result = new Logic.DTOs.Request.UpdateItemRequestDTO(
        //            new Logic.DTOs.Request.UpdateItemRequestDTO.UpdateItemRequestMetadata(nazivproizvoda)
        //        )
        //    {
        //        Cijena = model.Cijena,
        //        Datumakcije = model.Datumakcije,
        //        IsMonitoredByUser = model.IsMonitoredByUser,
        //        Opis = model.Opis,
        //        RetailerId = model.RetailerId,
        //        DeleteCurrentURLdoslike = model.DeleteCurrentURLdoslike,
        //        UpdatedByUserId = GetUserId(),
        //        URLdoslike = 
        //            model.URLdoslike == null ? 
        //                null 
        //                : 
        //                Path.Combine(ITEM_IMAGE_BASE_RELATIVE_PATH, Guid.NewGuid() + "_" + model.URLdoslike.FileName)
        //    };

        //    return result;
        //}

        private GetSingleItemResponseWebApiModel MapToGetSingleItemResponse(GetItemResponseDTO getItemResponseDTO)
        {
            var model = new GetSingleItemResponseWebApiModel()
            {
                Monitoring = new GetSingleItemResponseWebApiModel.MonitoredItemWebApiModel()
                {
                    IsMonitoredByUser = getItemResponseDTO.Monitoring.IsMonitoredByUser,
                    StartedMonitoringAtUtc = getItemResponseDTO.Monitoring.StartedMonitoringAtUtc
                },
                Item = new GetSingleItemResponseWebApiModel.ItemWebApiModel()
                {
                    Category = new GetSingleItemResponseWebApiModel.ItemWebApiModel.CategoryWebApiModel()
                    {
                        Id = getItemResponseDTO.Category.Id,
                        Name = getItemResponseDTO.Category.Name,
                        Priority = getItemResponseDTO.Category.Priority
                    },
                    Cijena = getItemResponseDTO.Cijena,
                    Datumakcije = getItemResponseDTO.Datumakcije,
                    IsEnabled = getItemResponseDTO  .IsEnabled,
                    Nazivproizvoda = getItemResponseDTO .Nazivproizvoda,
                    Opis = getItemResponseDTO.Opis,
                    Retailer = new GetSingleItemResponseWebApiModel.ItemWebApiModel.RetailerWebApiModel()
                    {
                        Id = getItemResponseDTO.Retailer.Id,
                        LogoImageUrl = string.IsNullOrEmpty(getItemResponseDTO.Retailer.LogoImageUrl) 
                            ? 
                            null 
                            : 
                            Path.Combine(ITEM_IMAGE_BASE_RELATIVE_PATH, getItemResponseDTO.Retailer.LogoImageUrl),
                        Name = getItemResponseDTO.Retailer.Name,
                        Priority = getItemResponseDTO.Retailer.Priority
                    },
                    URLdoslike = string.IsNullOrEmpty(getItemResponseDTO.URLdoslike) 
                    ? 
                    null 
                    : 
                    Path.Combine(Paths.FILES_BASE_URL, getItemResponseDTO.URLdoslike)
                }
            };

            return model;
        }

        private GetMultipleItemsResponseWebApiModel MapToGetMultipleItemsResponse(GetItemsResponseDTO getItemsResponseDTO)
        {
            var model = new GetMultipleItemsResponseWebApiModel();

            if (getItemsResponseDTO.Items != null && getItemsResponseDTO.Items.Count() > 0)
            {
                List<GetMultipleItemsResponseWebApiModel.ItemWebApiModel> items = 
                    new List<GetMultipleItemsResponseWebApiModel.ItemWebApiModel>();

                foreach (var item in getItemsResponseDTO.Items)
                {
                    items.Add(new GetMultipleItemsResponseWebApiModel.ItemWebApiModel()
                    {
                        Cijena = item.Cijena,
                        Category = new GetMultipleItemsResponseWebApiModel.ItemWebApiModel.CategoryWebApiModel()
                        {
                            Id = item.Category.Id,
                            Name = item.Category.Name
                        },
                        Datumakcije = item.Datumakcije,
                        IsEnabled = item.IsEnabled,
                        Monitoring = new GetMultipleItemsResponseWebApiModel.ItemWebApiModel.MonitoredItemWebApiModel()
                        {
                            IsMonitoredByCurrentUser = item.Monitoring.IsMonitoredByCurrentUser,
                            TotalUsersMonitoringThisItem = item.Monitoring.TotalUsersMonitoringThisItem
                        },
                        Nazivproizvoda = item.Nazivproizvoda,
                        Opis = item.Opis,
                        Retailer = new GetMultipleItemsResponseWebApiModel.ItemWebApiModel.RetailerWebApiModel()
                        {
                            Id = item.Retailer.Id,
                            Name = item.Retailer.Name,
                            Priority = item.Retailer.Priority
                        },
                        URLdoslike = string.IsNullOrEmpty(item.URLdoslike) ? null : Path.Combine(Paths.FILES_BASE_URL, item.URLdoslike)
                    });
                }

                model.Items = items;
            }

            model.PageInfo = new GetMultipleItemsResponseWebApiModel.PageWebApiModel()
            {
                Total = getItemsResponseDTO.PageInfo.Total
            };

            return model;
        }

        private CreateItemResponseWebApiModel MapToCreateItemResponse(CreateItemResponseDTO createItemResponseDTO)
        {
            var model = new CreateItemResponseWebApiModel()
            {
                Category = new CreateItemResponseWebApiModel.CategoryWebApiModel()
                {
                    Id = createItemResponseDTO.Category.Id,
                    Name = createItemResponseDTO.Category.Name,
                    Priority = createItemResponseDTO.Category.Priority
                },
                Retailer = new CreateItemResponseWebApiModel.RetailerWebApiModel()
                {
                    Priority = createItemResponseDTO.Retailer.Priority,
                    Name = createItemResponseDTO.Retailer.Name,
                    Id = createItemResponseDTO.Retailer.Id,
                    LogoImageUrl = Path.Combine(Paths.FILES_BASE_URL, createItemResponseDTO.Retailer.LogoImageUrl)
                },
                Cijena = createItemResponseDTO.Cijena,
                DatumAkcije = createItemResponseDTO.Datumakcije,
                IsEnabled = createItemResponseDTO.IsEnabled,
                Nazivproizvoda = createItemResponseDTO.Nazivproizvoda,
                Opis = createItemResponseDTO.Opis,
                URLdoslike = string.IsNullOrEmpty(createItemResponseDTO.URLdoslike) ? null : Path.Combine(Paths.FILES_BASE_URL, createItemResponseDTO.URLdoslike)
            };

            return model;
        }

        #endregion


        #region Permissions

        private async Task<bool> CanToggleMonitoredItemAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanToggleItemEnabledDisabledStatusAsync(HttpContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanSearchItemsAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanUpdateItem(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetMultipleItemsAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanDeleteItemAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanCreateItemAsync(HttpContext httpContext)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanGetItemAsync(HttpContext httpContext)
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
    }
}
