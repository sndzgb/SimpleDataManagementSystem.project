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
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [ValidateAntiForgeryToken]
    public class EditRetailerModel : BasePageModel<UpdateRetailerViewModel>
    {
        private readonly IRetailersService _retailersService;
        private readonly IAuthorizationService _authorizationService;

        public EditRetailerModel(IRetailersService retailersService, IAuthorizationService authorizationService)
        {
            _retailersService = retailersService;
            _authorizationService = authorizationService;
        }


        [FromRoute]
        public int RetailerId { get; set; }

        public GetSingleRetailerResponseViewModel Retailer { get; set; }


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
            Retailer = await _retailersService.GetSingleRetailerAsync(RetailerId, cancellationToken);

            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            await _retailersService.UpdateRetailerAsync(RetailerId, Model, cancellationToken);

            return RedirectToPage("/Retailers/Retailers");
        }
    }
}
