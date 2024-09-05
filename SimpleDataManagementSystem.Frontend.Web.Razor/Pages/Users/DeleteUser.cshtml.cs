using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public class DeleteUserModel : PageModel
    {
        private readonly IUsersService _usersService;


        public DeleteUserModel(IUsersService usersService)
        {
            _usersService = usersService;
        }


        [FromRoute]
        public int UserId { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDeleteUser()
        {
            await _usersService.DeleteUserAsync(UserId);

            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
