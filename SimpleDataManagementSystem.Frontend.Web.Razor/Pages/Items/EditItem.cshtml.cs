using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Diagnostics;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [ValidateAntiForgeryToken]
    public class EditItemModel : BasePageModel<UpdateItemViewModel>
    {
        private readonly IItemsService _itemsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IRetailersService _retailersService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<EditItemModel> _logger;


        public EditItemModel(
            IItemsService itemsService, 
            ICategoriesService categoriesService, 
            IRetailersService retailersService, 
            IAuthorizationService authorizationService,
            ILogger<EditItemModel> logger)
        {
            _itemsService = itemsService;
            _categoriesService = categoriesService;
            _retailersService = retailersService;
            _authorizationService = authorizationService;
            _logger = logger;
        }


        private string _itemId;

        [FromRoute]
        public string ItemId 
        { 
            get
            {
                return _itemId;
            }
            set
            {
                _itemId = Uri.UnescapeDataString(value);
            } 
        }

        public ItemViewModel Item { get; set; }

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

            tasks.Add(Task.Run(async () => Item = await _itemsService.GetItemByIdAsync(ItemId)));
            tasks.Add(Task.Run(async () => AvailableRetailers = await _retailersService.GetAllRetailersAsync(Int32.MaxValue, 1)));
            tasks.Add(Task.Run(async () => AvailableCategories = await _categoriesService.GetAllCategoriesAsync(Int32.MaxValue, 1)));

            await Task.WhenAll(tasks);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _itemsService.UpdateItemAsync(ItemId, Model);

            return RedirectToPage("/Items/Items");
        }

        public async Task<IActionResult> OnPostDeleteImage()
        {
            await _itemsService.UpdateItemPartialAsync(ItemId);
            
            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
