using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Services;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Write;
using System.Globalization;
using System.Reflection;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private const string IMAGE_BASE_PATH = "Images\\Items";

        private readonly IItemsService _itemsService;
        //private readonly IFilesService _filesService; // TODO


        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }


        [HttpPost]
        public async Task<IActionResult> AddNewItem([FromForm] NewItemWebApiModel newItemWebApiModel)
        {
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
                Nazivretailera = newItemWebApiModel.Nazivretailera,
                Cijena = decimal.Parse(newItemWebApiModel.Cijena, CultureInfo.InvariantCulture),
                Opis = newItemWebApiModel.Opis
            });

            if (newItemWebApiModel.URLdoslike != null)
            {
                FilesService.Upload(imageUrlPath, newItemWebApiModel.URLdoslike.OpenReadStream());

                // demo only; eg.: "D:\repos\SimpleDataManagementSystem\SimpleDataManagementSystem.Backend.WebAPI\bin\Debug\net7.0"
                //string assDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                //using (var fileStream = System.IO.File.Create(Path.Combine(assDirPath, imageUrlPath)))
                //{
                //    newItemWebApiModel.URLdoslike.CopyTo(fileStream);
                //}
            }

            return Created($"api/items/{newItemId}", newItemId); //"NewlyCreatedRetailerDTO"
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem([FromRoute] string itemId)
        {
            var item = await _itemsService.GetItemByIdAsync(itemId);

            await _itemsService.DeleteItemAsync(itemId);

            if ((item.URLdoslike != null) || !(string.IsNullOrEmpty(item.URLdoslike)))
            {
                FilesService.Delete(item.URLdoslike);
            }

            return Ok();
        }

        private bool CheckInvalidInput(string stringToCheck, IEnumerable<char> allowedChars)
        {
            return stringToCheck.All(allowedChars.Contains);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem([FromRoute] string itemId, [FromForm] UpdateItemWebApiModel updateItemWebApiModel)
        {
            // check if null, delete image & null DB path
            string imageUrlPath = string.Empty;

            if (updateItemWebApiModel.URLdoslike != null)
            {
                imageUrlPath = Path.Combine(IMAGE_BASE_PATH, Guid.NewGuid() + "_" + updateItemWebApiModel.URLdoslike.FileName);
            }

            // check if '.' and replace with ','
            // OPTIONAL: check if exists - '.' or ',' and append
            var allowedCharacters = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.' };
            var valid = CheckInvalidInput(updateItemWebApiModel.Cijena, allowedCharacters);


            await _itemsService.UpdateItemAsync(itemId, new UpdateItemDTO()
            {
                Opis = updateItemWebApiModel.Opis,
                Nazivretailera = updateItemWebApiModel.Nazivretailera,
                Cijena = decimal.Parse(updateItemWebApiModel.Cijena),
                //Cijena = decimal.Parse(updateItemWebApiModel.Cijena, CultureInfo.InvariantCulture),
                //Cijena = updateItemWebApiModel.Cijena,
                Datumakcije = updateItemWebApiModel.Datumakcije,
                Kategorija = updateItemWebApiModel.Kategorija,
                Nazivproizvoda = updateItemWebApiModel.Nazivproizvoda,
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
            var item = await _itemsService.GetItemByIdAsync(itemId);

            var model = new ItemWebApiModel()
            {
                Cijena = item.Cijena,
                Datumakcije = item.Datumakcije,
                Kategorija = item.Kategorija,
                Nazivproizvoda = item.Nazivproizvoda,
                Nazivretailera = item.Nazivretailera,
                Opis = item.Opis,
                URLdoslikeUri = Path.Combine(Paths.FILES_BASE_URL, item.URLdoslike)
            };

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems([FromQuery] int? take = 8, [FromQuery] int? page = 1)
        {
            var items = await _itemsService.GetAllItemsAsync(take, page);

            var models = new List<ItemWebApiModel>();

            if (items != null)
            {
                foreach (var item in items)
                {
                    models.Add(new ItemWebApiModel()
                    {
                        Cijena = item.Cijena,
                        Datumakcije = item.Datumakcije,
                        Kategorija = item.Kategorija,
                        Nazivproizvoda = item.Nazivproizvoda,
                        Nazivretailera = item.Nazivretailera,
                        Opis = item.Opis,
                        URLdoslikeUri = Path.Combine(Paths.FILES_BASE_URL, item.URLdoslike)
                    });
                }
            }

            return Ok(models);
        }
    }
}
