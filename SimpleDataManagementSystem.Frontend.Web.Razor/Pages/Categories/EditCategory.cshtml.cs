using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    [Authorize(Roles = "Admin,Employee")]
    public class EditCategoryModel : PageModel
    {
        private readonly ICategoriesService _categoriesService;


        public EditCategoryModel(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        [FromRoute]
        public int CategoryId { get; set; }

        public CategoryViewModel Category { get; set; }

        [BindProperty]
        public UpdateCategoryViewModel UpdatedCategory { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            Category = await _categoriesService.GetCategoryByIdAsync(CategoryId);

            return null;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                await _categoriesService.UpdateCategoryAsync(CategoryId, UpdatedCategory);

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
