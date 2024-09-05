using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface ICategoriesService
    {
        Task<int> AddNewCategoryAsync(NewCategoryViewModel newCategoryViewModel);
        Task<CategoriesViewModel> GetAllCategoriesAsync(int? take = 8, int? page = 1);
        Task<CategoryViewModel> GetCategoryByIdAsync(int categoryId);
        Task UpdateCategoryAsync(int categoryId, UpdateCategoryViewModel updateCategoryViewModel);
        Task DeleteCategoryAsync(int categoryId);
    }
}
