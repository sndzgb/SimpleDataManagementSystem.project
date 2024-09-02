using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "Admin,Employee")]
    public class DeleteRetailerModel : PageModel
    {
        private readonly IRetailersService _retailersService;


        public DeleteRetailerModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        public int RetailerId { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDeleteRetailer(int retailerId)
        {
            RetailerId = retailerId;

            await _retailersService.DeleteRetailerAsync(RetailerId);

            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
