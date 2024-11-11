using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    public class CreateUserModel : BasePageModel<NewUserViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;
        private readonly IAuthorizationService _authorizationService;


        public CreateUserModel(IUsersService usersService, IRolesService rolesService, IAuthorizationService authorizationService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
            _authorizationService = authorizationService;
        }


        public List<RoleViewModel> AvailableRoles { get; set; }


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


        public async Task<IActionResult> OnGet()
        {
            var roles = await _rolesService.GetAllRolesAsync();

            // TODO filter roles on server!
            AvailableRoles = roles.Where(x => x.Id != (int)Roles.Admin).ToList(); // creating new admins not allowed

            return Page();
        }

        public async Task<IActionResult> OnPost(NewUserViewModel newUserViewModel)
        {
            var newUserId = await _usersService.AddNewUserAsync(newUserViewModel);

            return RedirectToPage("/Users/Users");
        }
    }
}
