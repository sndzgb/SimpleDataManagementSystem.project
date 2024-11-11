using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [ValidateAntiForgeryToken]
    public class CreateRetailerModel : BasePageModel<NewRetailerViewModel>
    {
        private readonly IRetailersService _retailersService;
        private readonly IAuthorizationService _authorizationService;


        public CreateRetailerModel(IRetailersService retailersService, IAuthorizationService authorizationService)
        {
            _retailersService = retailersService;
            _authorizationService = authorizationService;
        }


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
            return Page();
        }

        public async Task<IActionResult> OnPost(NewRetailerViewModel newRetailerViewModel)
        {
            var newUserId = await _retailersService.AddNewRetailerAsync(newRetailerViewModel);

            return RedirectToPage("/Retailers/Retailers");
        }
    }
}
