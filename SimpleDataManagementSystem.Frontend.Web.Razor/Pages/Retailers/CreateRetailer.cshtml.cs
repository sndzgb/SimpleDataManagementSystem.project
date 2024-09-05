using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [Authorize(Roles = "Admin,Employee")]
    [ValidateAntiForgeryToken]
    public class CreateRetailerModel : PageModel
    {
        private readonly IRetailersService _retailersService;


        public CreateRetailerModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        [BindProperty]
        public NewRetailerViewModel NewRetailer { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            return null;
        }

        public async Task<IActionResult> OnPost(NewRetailerViewModel newRetailerViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                var newUserId = await _retailersService.AddNewRetailerAsync(newRetailerViewModel);

                return RedirectToPage("/Retailers/Retailers");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }
    }
}
