using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    [Authorize(Policy = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly IUsersService _usersService;


        public UsersModel(IUsersService usersService)
        {
            _usersService = usersService;
        }


        public List<UserViewModel> Users { get; set; }

        [BindProperty(Name = "pagenr", SupportsGet = true)]
        public int PageNr { get; set; } = 1;

        [BindProperty(Name = "ipp", SupportsGet = true)]
        public int ItemsPerPage { get; set; } = 8;


        public async Task OnGet()
        {
            Users = await _usersService.GetAllUsersAsync(ItemsPerPage, PageNr);

            return;
        }

        public void OnPost()
        {
        }
    }
}
