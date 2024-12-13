using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    public class DetailsModel : BasePageModel<GetSingleUserResponseViewModel>
    {
        private readonly IAccountsService _accountsService;
        private readonly IAuthorizationService _authorizationService;


        public DetailsModel(IAccountsService accountsService, IAuthorizationService authorizationService)
        {
            _accountsService = accountsService;
            _authorizationService = authorizationService;
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // TODO

            base.OnPageHandlerExecuting(context);
        }


        public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
        {
            Model = await _accountsService.GetAccountDetailsAsync(cancellationToken);
            return Page();
        }
    }
}
