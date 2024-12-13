using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [ValidateAntiForgeryToken]
    public class DeleteItemModel : BasePageModel<GetSingleItemResponseViewModel>
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

        public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
        {
            Model = await _itemsService.GetSingleItemAsync(ItemId, cancellationToken);

            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            await _itemsService.DeleteItemAsync(ItemId, cancellationToken);

            return RedirectToPage("/Items/Items");
        }
    }
}
