using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using SimpleDataManagementSystem.Shared.Common.Policies;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Users
{
    [ValidateAntiForgeryToken]
    public class DeleteUserModel : BasePageModel<GetSingleUserResponseViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IAuthorizationService _authorizationService;


        public DeleteUserModel(IUsersService usersService, IAuthorizationService authorizationService)
        {
            _usersService = usersService;
            _authorizationService = authorizationService;
        }


        [FromRoute]
        public int UserId { get; set; }

        
        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin };

            AuthorizationResult isInRole = await _authorizationService.AuthorizeAsync(
                HttpContext.User,
                new { roles },
                Policies.PolicyNames.UserIsInRole
            );

            AuthorizationResult isResourceOwner = await _authorizationService.AuthorizeAsync(
                HttpContext.User,
                UserId,
                Policies.PolicyNames.UserIsResourceOwner
            );

            if (!isInRole.Succeeded && !isResourceOwner.Succeeded)
            {
                context.Result = Forbid();
                return;
            }

            base.OnPageHandlerExecuting(context);
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            await _usersService.DeleteUserAsync(UserId, cancellationToken);

            return RedirectToPage("/Users/Users");
        }

        public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
        {
            Model = await _usersService.GetSingleUserAsync(UserId, cancellationToken);

            return Page();
        }
    }
}
