using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Helpers;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    public class LoginModel : BasePageModel<UserLogInRequestViewModel>
    {
        private readonly IAccountsService _accountsService;


        public LoginModel(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
        }


        public async Task<IActionResult> OnGet()
        {
            if (HttpContext!.User!.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(string username, string password)
        {
            var authTokenViewModel = await _accountsService.LogInAsync(username, password);

            if (authTokenViewModel?.Jwt == null)
            {
                Error = new ErrorViewModel(StatusCodes.Status401Unauthorized, "Login failed.", null);

                return await OnGet();
            }
            

            var claims = ClaimsHelper.GetClaimsFromToken(authTokenViewModel.Jwt);

            var identity = new ClaimsIdentity(claims, Cookies.AuthenticationCookie.Name);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(Cookies.AuthenticationCookie.Name, claimsPrincipal);

            if (Convert.ToBoolean(claims.Where(x => x.Type == ExtendedClaims.Type.IsPasswordChangeRequired).FirstOrDefault().Value))
            {
                var userId = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                ViewData["UserId"] = userId;

                return LocalRedirect($"/Account/Password-Change");
            }

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
