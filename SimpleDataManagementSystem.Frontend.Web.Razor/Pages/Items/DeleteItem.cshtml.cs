using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [ValidateAntiForgeryToken]
    public class DeleteItemModel : BasePageModel<ItemViewModel>
    {
        private readonly IItemsService _itemsService;
        private readonly IAuthorizationService _authorizationService;


        public DeleteItemModel(IItemsService itemsService, IAuthorizationService authorizationService)
        {
            _itemsService = itemsService;
            _authorizationService = authorizationService;
        }


        [FromRoute]
        public string ItemId { get; set; }


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
            Model = await _itemsService.GetItemByIdAsync(ItemId);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _itemsService.DeleteItemAsync(ItemId);

            return RedirectToPage("/Items/Items");
        }
    }
}
