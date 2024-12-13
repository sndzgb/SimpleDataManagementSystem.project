using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    [ValidateAntiForgeryToken]
    public class DeleteCategoryModel : BasePageModel<GetSingleCategoryResponseViewModel>
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IAuthorizationService _authorizationService;


        public DeleteCategoryModel(ICategoriesService categoriesService, IAuthorizationService authorizationService)
        {
            _categoriesService = categoriesService;
            _authorizationService = authorizationService;
        }


        [FromRoute]
        public int CategoryId { get; set; }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            int[] roles = new int[] { (int)Roles.Admin, (int)Roles.Employee };

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


        public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
        {
            Model = await _categoriesService.GetSingleCategoryAsync(CategoryId, cancellationToken);

            return Page();
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            await _categoriesService.DeleteCategoryAsync(CategoryId, cancellationToken);

            return RedirectToPage("/Categories/Categories");
        }
    }
}
