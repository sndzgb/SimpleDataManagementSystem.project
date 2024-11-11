using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Helpers;
using SimpleDataManagementSystem.Backend.WebAPI.Services;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Linq;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private const string IMAGE_BASE_PATH = "Images\\Items";

        private readonly IItemsService _itemsService;
        private readonly IAuthorizationService _authorizationService;
        //private readonly IFilesService _filesService; // TODO


        public ItemsController(IItemsService itemsService, IAuthorizationService authorizationService)
        {
            _itemsService = itemsService;
            _authorizationService = authorizationService;
        }


        [HttpPost]
        public async Task<IActionResult> AddNewItem(
                //IFormFile URLdoslike, IFormCollection formCollection
                [FromForm] NewItemWebApiModel newItemWebApiModel
            //NewItemWebApiModel newItemWebApiModel,
            //IFormFile URLdoslike
            )
        {
            //var formCollection = await Request.ReadFormAsync();
            //var file = formCollection.Files.First();

            //var f = Request.Form.Files;
            //var file = Request.Form.Files[0];

            // TEST
            //return BadRequest(new ErrorWebApiModel(400, "Invalid model.", new List<string>()
            //{
            //    "Item 1", "Item 2"
            //}));

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

            string? imageUrlPath = null; // string.Empty;

            if (newItemWebApiModel.URLdoslike != null)
            {
                imageUrlPath = Path.Combine(IMAGE_BASE_PATH, Guid.NewGuid() + "_" + newItemWebApiModel.URLdoslike.FileName);
            }

            string newItemId = await _itemsService.AddNewItemAsync(new NewItemDTO()
            {
                URLdoslike = imageUrlPath,
                Nazivproizvoda = newItemWebApiModel.Nazivproizvoda,
                Kategorija = newItemWebApiModel.Kategorija,
                Datumakcije = newItemWebApiModel.Datumakcije,
                RetailerId = newItemWebApiModel.RetailerId,
                Cijena = DecimalHelpers.GetDecimalFromString(newItemWebApiModel.Cijena),
                Opis = newItemWebApiModel.Opis
            });

            if (newItemWebApiModel.URLdoslike != null)
            {
                FilesService.Upload(imageUrlPath, newItemWebApiModel.URLdoslike.OpenReadStream());
            }

            // TODO get newly created item from DB, and return it to the client
            return Created($"api/items/{newItemId}", newItemId);
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] string itemId)
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

            if (itemId == null)
            {
                return BadRequest(new ErrorWebApiModel(StatusCodes.Status400BadRequest, "Item ID is required", null));
            }

            itemId = Uri.UnescapeDataString(itemId);

            // get item so we can get image path
            var item = await _itemsService.GetItemByIdAsync(itemId);

            await _itemsService.DeleteItemAsync(itemId);

            if ((item.URLdoslike != null) || !(string.IsNullOrEmpty(item.URLdoslike)))
            {
                FilesService.Delete(item.URLdoslike);
            }

            return Ok();
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem([FromRoute] string itemId, [FromForm] UpdateItemWebApiModel updateItemWebApiModel)
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

            itemId = Uri.UnescapeDataString(itemId);

            // check if null, delete image & null DB path
            string? imageUrlPath = null;

            if (updateItemWebApiModel.URLdoslike != null)
            {
                imageUrlPath = Path.Combine(IMAGE_BASE_PATH, Guid.NewGuid() + "_" + updateItemWebApiModel.URLdoslike.FileName);
            }

            await _itemsService.UpdateItemAsync(itemId, new UpdateItemDTO()
            {
                Opis = updateItemWebApiModel.Opis,
                Cijena = DecimalHelpers.GetDecimalFromString(updateItemWebApiModel.Cijena),
                RetailerId = updateItemWebApiModel.RetailerId,
                Datumakcije = updateItemWebApiModel.Datumakcije,
                Kategorija = updateItemWebApiModel.Kategorija,
                URLdoslike = imageUrlPath
            });

            if (updateItemWebApiModel.URLdoslike != null)
            {
                // save image
                FilesService.Upload(imageUrlPath, updateItemWebApiModel.URLdoslike.OpenReadStream());
            }

            return Ok();
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById([FromRoute] string itemId)
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

            //await Task.Delay(2000);

            itemId = Uri.UnescapeDataString(itemId);

            var item = await _itemsService.GetItemByIdAsync(itemId);

            if (item == null)
            {
                return NotFound(new ErrorWebApiModel(StatusCodes.Status404NotFound, "The requested resource was not found.", null));
            }

            var model = new ItemWebApiModel()
            {
                RetailerId = item.RetailerId,
                Cijena = item.Cijena,
                Datumakcije = item.Datumakcije,
                Kategorija = item.Kategorija,
                Nazivproizvoda = item.Nazivproizvoda,
                Nazivretailera = item.Nazivretailera,
                Opis = item.Opis,
                URLdoslikeUri = string.IsNullOrEmpty(item.URLdoslike) ? null : Path.Combine(Paths.FILES_BASE_URL, item.URLdoslike)
            };

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromQuery] int? take = 8, [FromQuery] int? page = 1)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                User,
                new { roles },
                Shared.Common.Constants.Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                return new ObjectResult(new ErrorWebApiModel(StatusCodes.Status403Forbidden, null, null))
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            var items = await _itemsService.GetAllItemsAsync(take, page);

            var model = new ItemsWebApiModel();

            model.PageInfo.Total = items.PageInfo.Total;
            model.PageInfo.Take = items.PageInfo.Take;
            model.PageInfo.Page = items.PageInfo.Page;

            if (items.Items != null)
            {
                foreach (var item in items.Items)
                {
                    model.Items.Add(new ItemWebApiModel()
                    {
                        Cijena = item.Cijena,
                        Datumakcije = item.Datumakcije,
                        Kategorija = item.Kategorija,
                        Nazivproizvoda = item.Nazivproizvoda,
                        Nazivretailera = item.Nazivretailera,
                        Opis = item.Opis,
                        URLdoslikeUri = string.IsNullOrEmpty(item.URLdoslike) ? null : Path.Combine(Paths.FILES_BASE_URL, item.URLdoslike)
                    });
                }
            }

            return Ok(model);
        }

        [HttpPatch("{itemId}")]
        public async Task<IActionResult> PatchItem([FromRoute] string itemId)
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

            itemId = Uri.UnescapeDataString(itemId);

            var item = await _itemsService.GetItemByIdAsync(itemId);

            if (item == null)
            {
                return BadRequest($"Item with ID '{itemId}' was not found");
            }

            await _itemsService.UpdateItemPartialAsync(item.Nazivproizvoda);

            FilesService.Delete(item.URLdoslike);

            return Ok();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchItems(
            int page, int take, string? searchQuery, string? sortBy, 
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return 
                    new ObjectResult(
                        new ErrorWebApiModel(
                            StatusCodes.Status400BadRequest,
                            "Search field is required.",
                            null
                        )
                    )
                    { 
                        StatusCode = StatusCodes.Status400BadRequest 
                    };
            }

            SearchableItemSortOrder sort = SearchableItemSortOrder.NazivproizvodaDesc;

            if (Int32.TryParse(sortBy, out int num))
            {
                sort = (SearchableItemSortOrder)num;
            }

            var result = 
                await _itemsService.SearchItemsAsync(
                    new ItemSearchRequestDTO()
                    {
                        Page = page,
                        Take = take,
                        SearchQuery = searchQuery,
                        SortBy = sort
                    }, 
                    cancellationToken
            );

            var model = new ItemSearchResponseWebApiModel()
            {
                Items = result.Items.Select(x => new ItemWebApiModel()
                {
                    Cijena = x.Cijena,
                    Nazivproizvoda = x.Nazivproizvoda,
                    URLdoslikeUri = string.IsNullOrEmpty(x.URLdoslike) ? null : Path.Combine(Paths.FILES_BASE_URL, x.URLdoslike)
                }).ToList(),
                PageInfo = new PagedWebApiModel()
                {
                    Page = result.PageInfo.Page,
                    Take = result.PageInfo.Take,
                    Total = result.PageInfo.Total
                },
                Request = new ItemSearchRequestWebApiModel()
                {
                    Page = page,
                    Take = take,
                    SearchQuery = searchQuery,
                    SortBy = sort
                }
            };

            return Ok(model);
        }
    }
}
