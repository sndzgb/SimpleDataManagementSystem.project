using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public class PasswordChangeModel : BasePageModel<UpdatePasswordViewModel>
    {
        private readonly IAccountsService _accountsService;
        private readonly IUsersService _usersService;


        public PasswordChangeModel(IAccountsService accountsService, IUsersService usersService)
        {
            _accountsService = accountsService;
            _usersService = usersService;
        }


        public async Task<IActionResult> OnGet()
        {
            if (!(User.Identity!.IsAuthenticated))
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            await _accountsService.UpdatePasswordAsync(Model?.OldPassword, Model?.NewPassword, cancellationToken);

            await HttpContext.SignOutAsync();

            return RedirectToPage("");
        }
    }
}
