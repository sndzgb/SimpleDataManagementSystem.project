using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    [IgnoreAntiforgeryToken]
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
            var user = await _accountsService.LogInAsync(username, password);

            if (user == null) 
            {
                return RedirectToPage("/Index");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, user!.Claims!.FirstOrDefault()!.Value),
                new Claim("Role", user!.Claims!.FirstOrDefault()!.Value)
            };

            var identity = new ClaimsIdentity(claims, "MyCookie");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookie", claimsPrincipal);

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
