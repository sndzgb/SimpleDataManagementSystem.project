using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
//using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Globalization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [Authorize(Roles = "Admin,Employee")]
    public class CreateItemModel : PageModel //SimpleDataManagementSystemPageModelBase
    {
        private readonly IItemsService _itemsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IRetailersService _retailersService;


        //public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        //{
        //    var exception = context.Exception;
        //}


        public CreateItemModel(IItemsService itemsService, ICategoriesService categoriesService, IRetailersService retailersService)
        {
            _itemsService = itemsService;
            _categoriesService = categoriesService;
            _retailersService = retailersService;
        }


        //[FromBody]
        [BindProperty]
        public NewItemViewModel NewItem { get; set; }

        public CategoriesViewModel AvailableCategories { get; set; }

        public RetailersViewModel AvailableRetailers { get; set; }
        
        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => AvailableCategories = await _categoriesService.GetAllCategoriesAsync(int.MaxValue, 1)));
            tasks.Add(Task.Run(async () => AvailableRetailers = await _retailersService.GetAllRetailersAsync(int.MaxValue, 1)));

            await Task.WhenAll(tasks);

            return null;
        }


        public async Task<IActionResult> OnPost(NewItemViewModel newItemViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                var newItemId = await _itemsService.AddNewItemAsync(newItemViewModel);

                return RedirectToPage("/Items/Items");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }
    }
}
