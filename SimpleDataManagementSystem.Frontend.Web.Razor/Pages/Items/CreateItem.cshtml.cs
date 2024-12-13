using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Globalization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    public class CreateItemModel : BasePageModel<CreateItemRequestViewModel>
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


        public GetMultipleCategoriesResponseViewModel AvailableCategories { get; set; }

        public GetMultipleRetailersResponseViewModel AvailableRetailers { get; set; }


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


        public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => 
                AvailableCategories = await _categoriesService.GetMultipleCategoriesAsync(cancellationToken, int.MaxValue, 1))
            );
            tasks.Add(Task.Run(async () => 
                AvailableRetailers = await _retailersService.GetMultipleRetailersAsync(cancellationToken, int.MaxValue, 1))
            );

            await Task.WhenAll(tasks);

            return Page();
        }

        public async Task<IActionResult> OnPost(CreateItemRequestViewModel createItemRequestViewModel, CancellationToken cancellationToken)
        {
            var newItemId = await _itemsService.CreateItemAsync(createItemRequestViewModel, cancellationToken);

            return RedirectToPage("/Items/Items");
        }
    }
}
