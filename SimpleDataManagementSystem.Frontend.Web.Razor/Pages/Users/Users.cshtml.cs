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


        public UsersViewModel Users { get; set; }

        // TODO use properties instead of method parameters
        //[BindProperty(Name = "pagenr", SupportsGet = true)]
        //public int PageNr { get; set; } = 1;

        //[BindProperty(Name = "ipp", SupportsGet = true)]
        //public int ItemsPerPage { get; set; } = 8;


        public async Task OnGet([FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Users = await _usersService.GetAllUsersAsync(take, page);
            
            return;
        }

        public void OnPost()
        {
        }
    }
}
