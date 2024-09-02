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


        public List<RetailerViewModel> Retailers { get; set; }

        [BindProperty(Name = "pagenr", SupportsGet = true)]
        public int PageNr { get; set; } = 1;

        [BindProperty(Name = "ipp", SupportsGet = true)]
        public int ItemsPerPage { get; set; } = 8;


        public async Task OnGet()
        {
            Retailers = await _retailersService.GetAllRetailersAsync(ItemsPerPage, PageNr);

            return;
        }

        public void OnPost()
        {
        }
    }
}
