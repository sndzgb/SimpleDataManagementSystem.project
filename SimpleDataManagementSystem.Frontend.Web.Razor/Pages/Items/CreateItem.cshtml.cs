using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Globalization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    public class CreateItemModel : BasePageModel<NewItemViewModel>
    {
        private readonly IItemsService _itemsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IRetailersService _retailersService;
        private readonly IAuthorizationService _authorizationService;


        public CreateItemModel(
                IItemsService itemsService, 
                ICategoriesService categoriesService, 
                IRetailersService retailersService,
                IAuthorizationService authorizationService
            )
        {
            _itemsService = itemsService;
            _categoriesService = categoriesService;
            _retailersService = retailersService;
            _authorizationService = authorizationService;
        }


        [FromQuery(Name = "test")]
        public string? Test { get; set; }

        public CategoriesViewModel AvailableCategories { get; set; }

        public RetailersViewModel AvailableRetailers { get; set; }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                HttpContext.User,
                new { roles },
                Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                context.Result = Forbid();
                return;
            }

            base.OnPageHandlerExecuting(context);
        }


        public async Task<IActionResult> OnGet()
        {
            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => AvailableCategories = await _categoriesService.GetAllCategoriesAsync(int.MaxValue, 1)));
            tasks.Add(Task.Run(async () => AvailableRetailers = await _retailersService.GetAllRetailersAsync(int.MaxValue, 1)));

            await Task.WhenAll(tasks);

            return Page();
        }

        public async Task<IActionResult> OnPost(NewItemViewModel newItemViewModel)
        {
            var newItemId = await _itemsService.AddNewItemAsync(newItemViewModel);

            return RedirectToPage("/Items/Items");
        }
    }
}
