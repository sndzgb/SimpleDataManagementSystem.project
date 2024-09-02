using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Retailers
{
    [Authorize(Roles = "Admin,Employee")]
    public class CreateRetailerModel : PageModel
    {
        private readonly IRetailersService _retailersService;


        public CreateRetailerModel(IRetailersService retailersService)
        {
            _retailersService = retailersService;
        }


        [BindProperty]
        public NewRetailerViewModel NewRetailer { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(NewRetailerViewModel newRetailerViewModel)
        {
            //try
            //{
                var newUserId = await _retailersService.AddNewRetailerAsync(newRetailerViewModel);

                return RedirectToPage("/Retailers/Retailers");
            //}
            //catch (WebApiCallException wace)
            //{
            //    ViewData["Error"] = wace.Error;
            //    return Page();
            //}
        }
    }
}
