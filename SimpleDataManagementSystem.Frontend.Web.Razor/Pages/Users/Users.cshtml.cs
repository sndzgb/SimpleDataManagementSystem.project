using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    public class UsersModel : BasePageModel<UsersViewModel>
    {
        private readonly IUsersService _usersService;
        private readonly IAuthorizationService _authorizationService;


        public UsersModel(IUsersService usersService, IAuthorizationService authorizationService)
        {
            _usersService = usersService;
            _authorizationService = authorizationService;
        }


        public UsersViewModel Users { get; set; }

        // TODO use properties instead of method parameters
        //[BindProperty(Name = "pagenr", SupportsGet = true)]
        //public int PageNr { get; set; } = 1;

        //[BindProperty(Name = "ipp", SupportsGet = true)]
        //public int ItemsPerPage { get; set; } = 8;

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


        public async Task<IActionResult> OnGet([FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Users = await _usersService.GetAllUsersAsync(take, page);
            return Page();
        }
    }
}
