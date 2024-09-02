using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [Authorize(Roles = "Admin,Employee")]
    public class EditRetailerModel : PageModel
    {
        private readonly IRetailersService _retailersService;


        public EditRetailerModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        public int RetailerId { get; set; }

        public RetailerViewModel Retailer { get; set; }

        [BindProperty]
        public UpdateRetailerViewModel UpdatedRetailer { get; set; }


        public async Task OnGet(int retailerId)
        {
            RetailerId = retailerId;

            Retailer = await _retailersService.GetRetailerByIdAsync(retailerId);

            return;
        }

        public async Task<IActionResult> OnPost([FromRoute] int retailerId)
        {
            RetailerId = retailerId;

            await _retailersService.UpdateRetailerAsync(RetailerId, UpdatedRetailer);

            return RedirectToPage("/Retailers/Retailers");
        }
    }
}
