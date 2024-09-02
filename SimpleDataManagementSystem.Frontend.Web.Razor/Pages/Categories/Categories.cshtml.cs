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


        [BindProperty(Name = "pagenr", SupportsGet = true)]
        public int PageNr { get; set; } = 1;

        [BindProperty(Name = "ipp", SupportsGet = true)]
        public int ItemsPerPage { get; set; } = 8;

        public List<CategoryViewModel> Categories { get; set; }


        public async Task OnGet()
        {
            Categories = await _categoriesService.GetAllCategoriesAsync(ItemsPerPage, PageNr);

            return;
        }

        public void OnPost() 
        {
        }
    }
}
