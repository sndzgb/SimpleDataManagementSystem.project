using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    public class LocalizationModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(string culture, string? returnUrl) 
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
