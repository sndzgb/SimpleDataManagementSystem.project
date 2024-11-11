using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    public class CreateCategoryModel : BasePageModel<NewCategoryViewModel>
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IAuthorizationService _authorizationService;


        public CreateCategoryModel(ICategoriesService categoriesService, IAuthorizationService authorizationService)
        {
            _categoriesService = categoriesService;
            _authorizationService = authorizationService;
        }


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


        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(NewCategoryViewModel newCategoryViewModel)
        {
            var newCategory = await _categoriesService.AddNewCategoryAsync(newCategoryViewModel);

            return RedirectToPage("/Categories/Categories");
        }
    }
}
