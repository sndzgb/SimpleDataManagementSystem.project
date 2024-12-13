using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Threading;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Items
{
    [ValidateAntiForgeryToken]
    public class ItemsModel : BasePageModel<GetMultipleItemsResponseViewModel>
    {
        private readonly IItemsService _itemsService;
        private readonly IAuthorizationService _authorizationService;
        

        public ItemsModel(
                IItemsService itemsService, 
                IAuthorizationService authorizationService
            )
        {
            _itemsService = itemsService;
            _authorizationService = authorizationService;
        }


        [FromQuery(Name = "enabled_only")]
        public bool EnabledOnly { get; set; }


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


        public async Task OnGet(
                CancellationToken cancellationToken,
                [FromQuery(Name = "enabled_only")] bool enabledOnly = true, 
                [FromQuery] int take = 8, 
                [FromQuery] int page = 1
            )
        {
            EnabledOnly = enabledOnly;
            
            Model = await _itemsService.GetMultipleItemsAsync(cancellationToken, EnabledOnly, take, page);

            return;
        }

        public async Task<IActionResult> OnPostToggleItemEnabledDisabledStatus(string itemId, CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(HttpContext.RequestAborted, cancellationToken))
            {
                await _itemsService.ToggleItemEnabledDisabledStatusAsync(itemId, cancellationToken);
                
                return new JsonResult(null)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }
        
        public async Task<IActionResult> OnPostToggleMonitoredItem(string itemId, CancellationToken cancellationToken)
        {
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(HttpContext.RequestAborted, cancellationToken))
            {
                await _itemsService.ToggleMonitoredItemAsync(itemId, cts.Token);

                return new JsonResult(null)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
        }
    }
}
