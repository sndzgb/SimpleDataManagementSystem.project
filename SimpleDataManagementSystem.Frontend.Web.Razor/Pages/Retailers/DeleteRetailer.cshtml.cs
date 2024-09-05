using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Employee")]
    public class DeleteRetailerModel : PageModel
    {
        private readonly IRetailersService _retailersService;


        public DeleteRetailerModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        [FromRoute]
        public int RetailerId { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDeleteRetailer()
        {
            await _retailersService.DeleteRetailerAsync(RetailerId);

            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
