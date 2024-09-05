using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    [Authorize(Roles = "Admin")]
    public class CreateUserModel : PageModel
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;


        public CreateUserModel(IUsersService usersService, IRolesService rolesService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
        }


        [BindProperty]
        public NewUserViewModel NewUser { get; set; }

        public List<RoleViewModel> AvailableRoles { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var roles = await _rolesService.GetAllRolesAsync();

            AvailableRoles = roles.Where(x => x.Id != (int)Roles.Admin).ToList(); // creating new admins not allowed

            return null;
        }

        public async Task<IActionResult> OnPost(NewUserViewModel newUserViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                var newUserId = await _usersService.AddNewUserAsync(newUserViewModel);

                return RedirectToPage("/Users/Users");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }
    }
}
