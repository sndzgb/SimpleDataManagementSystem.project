using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Employee")]
    public class DeleteCategoryModel : PageModel
    {
        private readonly ICategoriesService _categoriesService;


        public DeleteCategoryModel(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        [FromRoute]
        public int CategoryId { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDeleteCategory()
        {
            await _categoriesService.DeleteCategoryAsync(CategoryId);

            return new JsonResult(null) { StatusCode = (int)HttpStatusCode.OK };
        }
    }
}
