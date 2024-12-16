using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Users
{
    public class CreateUserModel : BasePageModel<CreateUserViewModel>
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


        public GetAllRolesResponseViewModel AvailableRoles { get; set; }


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
            var roles = await _rolesService.GetAllRolesAsync(cancellationToken);

            AvailableRoles = new GetAllRolesResponseViewModel(); // creating new admins not allowed
            AvailableRoles.Roles = new List<GetAllRolesResponseViewModel.RolesViewModel>();
            AvailableRoles.PageInfo = roles.PageInfo; //new GetAllRolesResponseViewModel.();

            AvailableRoles.Roles.AddRange(roles.Roles.Where(x => x.Name.ToLower() != Roles.Admin.ToString().ToLower()).ToList());
			//AvailableRoles = roles.Roles.Where(x => x.Id != (int)Roles.Admin); // creating new admins not allowed

			return Page();
        }

        public async Task<IActionResult> OnPost(CreateUserViewModel createUserViewModel, CancellationToken cancellationToken)
        {
            var newUserId = await _usersService.CreateUserAsync(createUserViewModel, cancellationToken);

            return RedirectToPage("/Users/Users");
        }
    }
}
