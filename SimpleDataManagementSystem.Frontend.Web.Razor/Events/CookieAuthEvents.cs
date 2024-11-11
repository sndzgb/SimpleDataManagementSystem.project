using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Events
{
    public class CookieAuthEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = "/Forbidden";

            return base.RedirectToAccessDenied(context);
        }
    }
}
