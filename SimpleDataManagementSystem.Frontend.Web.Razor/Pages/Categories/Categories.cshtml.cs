using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    public class CategoriesModel : BasePageModel<GetMultipleCategoriesResponseViewModel>
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IAuthorizationService _authorizationService;


        public CategoriesModel(ICategoriesService categoriesService, IAuthorizationService authorizationService)
        {
            _categoriesService = categoriesService;
            _authorizationService = authorizationService;
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee, (int)Roles.User };

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(
                HttpContext.User,
                new { roles },
                Policies.PolicyNames.UserIsInRole
            );

            if (!authorizationResult.Succeeded)
            {
                context.Result = Forbid();
                return;
            }

            base.OnPageHandlerExecuting(context);
        }

        public async Task<IActionResult> OnGet(CancellationToken cancellationToken, [FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Model = await _categoriesService.GetMultipleCategoriesAsync(cancellationToken, take, page);

            return Page();
        }
    }
}
