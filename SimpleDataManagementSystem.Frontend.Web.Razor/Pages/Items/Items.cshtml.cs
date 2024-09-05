using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    public class ItemsModel : PageModel
    {
        private readonly IItemsService _itemsService;


        public ItemsModel(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }


        public ItemsViewModel Items { get; set; }


        public async Task OnGet([FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Items = await _itemsService.GetAllItemsAsync(take, page);

            return;
        }
    }
}
