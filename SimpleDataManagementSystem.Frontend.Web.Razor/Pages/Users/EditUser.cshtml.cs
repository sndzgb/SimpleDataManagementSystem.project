using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;


        public EditUserModel(IUsersService usersService, IRolesService rolesService)
        {
            _usersService = usersService;
            _rolesService = rolesService;
        }


        [FromRoute]
        public int UserId { get; set; }

        [BindProperty]
        public UpdateUserViewModel UpdatedUser { get; set; } // updated model

        public UserViewModel User { get; set; } // user details

        public List<RoleViewModel> AvailableRoles { get; set; } // roles to choose from

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => User = await _usersService.GetUserByIdAsync(UserId)));
            tasks.Add(Task.Run(async () => {
                AvailableRoles = await _rolesService.GetAllRolesAsync();
                AvailableRoles = AvailableRoles.Where(x => x.Id != (int)Roles.Admin).ToList();
            }));

            await Task.WhenAll(tasks);

            return null;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                await _usersService.UpdateUserAsync(UserId, UpdatedUser);

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
