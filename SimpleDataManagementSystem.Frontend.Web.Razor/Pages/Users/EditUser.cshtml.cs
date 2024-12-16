using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Users
{
    public class EditUserModel : BasePageModel<UpdateUserViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;
        private readonly IAuthorizationService _authorizationService;


        public EditUserModel(
                IUsersService usersService, 
                IRolesService rolesService, 
                IAuthorizationService authorizationService
            )
        {
            _usersService = usersService;
            _rolesService = rolesService;
            _authorizationService = authorizationService;
        }


        [FromRoute]
        public int UserId { get; set; }

        public GetSingleUserResponseViewModel User { get; set; } // user details

        public GetAllRolesResponseViewModel AvailableRoles { get; set; } // roles to choose from
        

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


        public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => User = await _usersService.GetSingleUserAsync(UserId, cancellationToken)));
            tasks.Add(Task.Run(async () => {
                AvailableRoles = await _rolesService.GetAllRolesAsync(cancellationToken);
                AvailableRoles.Roles = AvailableRoles.Roles.Where(x => x.Id != (int)Roles.Admin).ToList();
            }));

            await Task.WhenAll(tasks);

            return Page();
        }

        public async Task<IActionResult> OnPost(UpdateUserViewModel updateUserViewModel, CancellationToken cancellationToken)
        {
            await _usersService.UpdateUserAsync(UserId, updateUserViewModel, cancellationToken);

            return RedirectToPage("/Users/Users");
        }
    }
}
