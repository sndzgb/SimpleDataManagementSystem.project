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


        [BindProperty(Name = "pagenr", SupportsGet = true)]
        public int PageNr { get; set; } = 1;

        [BindProperty(Name = "ipp", SupportsGet = true)]
        public int ItemsPerPage { get; set; } = 8;

        public List<ItemViewModel> Items { get; set; }


        public async Task OnGet()
        {
            Items = await _itemsService.GetAllItemsAsync(ItemsPerPage, PageNr);
            
            return;
        }
    }
}
