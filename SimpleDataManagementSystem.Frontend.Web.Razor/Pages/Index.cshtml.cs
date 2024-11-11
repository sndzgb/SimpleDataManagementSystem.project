using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    public class IndexModel : BasePageModel<object>
    {
        private readonly ILogger<IndexModel> _logger;

        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}