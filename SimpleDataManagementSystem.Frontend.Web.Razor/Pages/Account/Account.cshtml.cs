using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Helpers;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
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


        public async Task<IActionResult> OnGet()
        {
            return null;
            //return RedirectToPage("/Index");
        }

        
        public ErrorViewModel Error { get; set; }

        [BindProperty]
        public CredentialViewModel Credential { get; set; }


        public async Task<IActionResult> OnPostLogout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToPage("/Account/Account"); // login
            //return RedirectToPage("/Index");
        }

        //async Task<IActionResult>
        public async Task<IActionResult> OnPost(string username, string password) //OnPostLogin
        {
            //try
            //{
                var token = await _accountsService.LogInAsync(username, password);

                if (token == null)
                {
                    Error = new ErrorViewModel(401, "Login failed.", null);

                    //ViewData["Error"] = "Login failed.";
                    //return;
                    //return Page();
                    return await OnGet();
                    //return RedirectToPage("/Index");
                }

                var claims = ClaimsHelper.GetClaimsFromToken(token);

                var identity = new ClaimsIdentity(claims, Cookies.AuthenticationCookie.Name);

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                if (Convert.ToBoolean(claims.Where(x => x.Type == ExtendedClaims.Type.IsPasswordChangeRequired).FirstOrDefault().Value))
                {
                    var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                    ViewData["UserId"] = userId;

                    return LocalRedirect($"/Account/{userId}/ChangePassword");

                    //return RedirectToPage("/Account/{UserId}/ChangePassword", new { UserId = userId });
                    //return new RedirectToPageResult("/Account/ChangePassword");
                    //return RedirectToPage("/Account/ChangePassword");
                }

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
            //}
            //catch (HttpRequestException e) when (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{

            //}
        }
    }
}
