using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [Authorize(Roles = "Admin,Employee")]
    [ValidateAntiForgeryToken]
    public class EditRetailerModel : PageModel
    {
        private readonly IRetailersService _retailersService;


        public EditRetailerModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        [FromRoute]
        public int RetailerId { get; set; }

        public RetailerViewModel Retailer { get; set; }

        [BindProperty]
        public UpdateRetailerViewModel UpdatedRetailer { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            Retailer = await _retailersService.GetRetailerByIdAsync(RetailerId);

            return null;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                await _retailersService.UpdateRetailerAsync(RetailerId, UpdatedRetailer);

                return RedirectToPage("/Retailers/Retailers");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }

        public async Task<IActionResult> OnPostDeleteImage([FromRoute] int retailerId)
        {
            // TODO use RetailerId property
            await _retailersService.UpdateRetailerPartialAsync(retailerId);

            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
