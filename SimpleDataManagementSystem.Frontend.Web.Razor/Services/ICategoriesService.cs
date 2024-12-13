using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface ICategoriesService
    {
        Task CreateCategoryAsync(CreateCategoryViewModel createCategoryViewModel, CancellationToken cancellationToken);
        Task<GetMultipleCategoriesResponseViewModel> GetMultipleCategoriesAsync(
            CancellationToken cancellationToken, int? take = 8, int? page = 1
        );
        Task<GetSingleCategoryResponseViewModel> GetSingleCategoryAsync(int categoryId, CancellationToken cancellationToken);
        Task UpdateCategoryAsync(int categoryId, UpdateCategoryViewModel updateCategoryViewModel, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(int categoryId, CancellationToken cancellationToken);
    }
}
