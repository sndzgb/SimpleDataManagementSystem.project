using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    public class RetailersModel : PageModel
    {
        private readonly IRetailersService _retailersService;
        

        public RetailersModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        public RetailersViewModel Retailers { get; set; }


        public async Task OnGet([FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Retailers = await _retailersService.GetAllRetailersAsync(take, page);

            return;
        }

        public void OnPost()
        {
        }
    }
}
