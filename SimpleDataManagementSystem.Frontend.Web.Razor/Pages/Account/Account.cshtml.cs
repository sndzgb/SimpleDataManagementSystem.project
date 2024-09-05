using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Helpers;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    [ValidateAntiForgeryToken]
    public class AccountModel : PageModel
    {
        private readonly IAccountsService _accountsService;
        

        public AccountModel(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostLogin(string username, string password)
        {
            var token = await _accountsService.LogInAsync(username, password);

            if (token == null)
            {
                return RedirectToPage("/Index");
            }

            var claims = ClaimsHelper.GetClaimsFromToken(token);

            var identity = new ClaimsIdentity(claims, Cookies.AuthenticationCookie.Name);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(Cookies.AuthenticationCookie.Name, claimsPrincipal);
            
            string returnUrl = HttpContext.Request.Query["returnUrl"];

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
