using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

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


        public int UserId { get; set; }

        [BindProperty]
        public UpdateUserViewModel UpdatedUser { get; set; } // updated model

        public UserViewModel User { get; set; } // user details

        public List<RoleViewModel> Roles { get; set; } // roles to choose from


        public async Task OnGet(int userId)
        {
            UserId = userId;

            var tasks = new List<Task>();

            tasks.Add(Task.Run(async () => User = await _usersService.GetUserByIdAsync(userId)));
            tasks.Add(Task.Run(async () => Roles = await _rolesService.GetAllRolesAsync()));

            await Task.WhenAll(tasks);

            return;
        }

        public async Task<IActionResult> OnPost(int userId)
        {
            UserId = userId;

            await _usersService.UpdateUserAsync(UserId, UpdatedUser);

            return RedirectToPage("/Users");
        }
    }
}
