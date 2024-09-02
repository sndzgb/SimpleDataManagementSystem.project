using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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


        public int CategoryId { get; set; }

        public CategoryViewModel Category { get; set; }

        [BindProperty]
        public UpdateCategoryViewModel UpdatedCategory { get; set; }


        public async Task OnGet(int categoryId)
        {
            CategoryId = categoryId;

            Category = await _categoriesService.GetCategoryByIdAsync(categoryId);

            return;
        }

        public async Task<IActionResult> OnPost([FromRoute] int categoryId)
        {
            CategoryId = categoryId;

            await _categoriesService.UpdateCategoryAsync(CategoryId, UpdatedCategory);

            return RedirectToPage("/Categories/Categories");
        }
    }
}
