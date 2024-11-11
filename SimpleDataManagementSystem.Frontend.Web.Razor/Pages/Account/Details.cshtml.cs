using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    public class DetailsModel : BasePageModel<UserViewModel>
    {
        private readonly IAccountsService _accountsService;


        public DetailsModel(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            base.OnPageHandlerExecuting(context);
        }


        public async Task<IActionResult> OnGet()
        {
            Model = await _accountsService.GetAccountDetailsAsync();
            return Page();
        }
    }
}
