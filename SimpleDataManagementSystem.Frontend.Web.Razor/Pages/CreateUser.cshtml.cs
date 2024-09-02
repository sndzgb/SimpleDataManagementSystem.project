using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

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


        public async Task OnGet()
        {
            var roles = await _rolesService.GetAllRolesAsync();

            AvailableRoles = roles.Where(x => x.Id != (int)Roles.Admin).ToList();
        }

        public async Task<IActionResult> OnPost(NewUserViewModel newUserViewModel)
        {
            //try
            //{
            //    if (!ModelState.IsValid)
            //    {
            //        var ms = ModelState.Values.SelectMany(v => v.Errors);
            //        ViewData["ModelState"] = ms;
            //        return Page();
            //    }
                
                var newUserId = await _usersService.AddNewUserAsync(newUserViewModel);

                return RedirectToPage("/Users");
            //}
            //catch (WebApiCallException wace)
            //{
            //    ViewData["Error"] = wace.Error;
            //    return Page();
            //}
        }
    }
}
