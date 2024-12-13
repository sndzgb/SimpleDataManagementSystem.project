using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    public class UsersModel : BasePageModel<GetMultipleUsersResponseViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IAuthorizationService _authorizationService;


        public UsersModel(IUsersService usersService, IAuthorizationService authorizationService)
        {
            _usersService = usersService;
            _authorizationService = authorizationService;
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin };

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


        public async Task<IActionResult> OnGet(CancellationToken cancellationToken, [FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Model = await _usersService.GetMultipleUsersAsync(cancellationToken, take, page);
            return Page();
        }
    }
}
