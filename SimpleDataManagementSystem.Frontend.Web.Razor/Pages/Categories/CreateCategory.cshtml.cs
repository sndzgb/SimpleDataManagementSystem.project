using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    [Authorize(Roles = "Admin,Employee")]
    public class CreateCategoryModel : PageModel
    {
        private readonly ICategoriesService _categoriesService;


        public CreateCategoryModel(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        [BindProperty]
        public NewCategoryViewModel NewCategory { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            return null;
        }

        public async Task<IActionResult> OnPost(NewCategoryViewModel newCategoryViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }
                var newUserId = await _categoriesService.AddNewCategoryAsync(newCategoryViewModel);

                return RedirectToPage("/Categories/Categories");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }
    }
}
