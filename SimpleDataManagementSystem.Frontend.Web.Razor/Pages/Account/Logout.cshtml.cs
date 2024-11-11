using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    public class LogoutModel : BasePageModel<object>
    {
        public LogoutModel()
        {
            
        }

        // ON GET REDIRECT TO INDEX.CSHTML

        public async Task<IActionResult> OnPost()
        {
            await HttpContext.SignOutAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}
