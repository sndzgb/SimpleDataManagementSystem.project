using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Categories
{
    public class CategoriesModel : PageModel
    {
        private readonly ICategoriesService _categoriesService;


        public CategoriesModel(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }


        public CategoriesViewModel Categories { get; set; }


        public async Task OnGet([FromQuery] int take = 8, [FromQuery] int page = 1)
        {
            Categories = await _categoriesService.GetAllCategoriesAsync(take, page);

            return;
        }

        public void OnPost() 
        {
        }
    }
}
