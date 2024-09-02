using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Globalization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [Authorize(Roles = "Admin,Employee")]
    public class CreateItemModel : PageModel
    {
        private readonly IItemsService _itemsService;
        private readonly ICategoriesService _categoriesService;

        
        public CreateItemModel(IItemsService itemsService, ICategoriesService categoriesService)
        {
            _itemsService = itemsService;
            _categoriesService = categoriesService;
        }


        [BindProperty]
        public NewItemViewModel NewItem { get; set; }

        public List<CategoryViewModel> AvailableCategories { get; set; }


        public async Task OnGet()
        {
            AvailableCategories = await _categoriesService.GetAllCategoriesAsync(int.MaxValue, 1);
        }

        public async Task<IActionResult> OnPost(NewItemViewModel newItemViewModel)
        {
            //try
            //{
                var newUserId = await _itemsService.AddNewItemAsync(newItemViewModel);

                return RedirectToPage("/Items/Items");
            //}
            //catch (WebApiCallException wace)
            //{
            //    ViewData["Error"] = wace.Error;
            //    return Page();
            //}
        }
    }
}
