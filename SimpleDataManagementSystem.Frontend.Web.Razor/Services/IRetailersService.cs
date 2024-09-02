
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IRetailersService
    {
        Task<int> AddNewRetailerAsync(NewRetailerViewModel newRetailerViewModel);
        Task<List<RetailerViewModel>> GetAllRetailersAsync(int? take = 8, int? page = 1);
        Task<RetailerViewModel> GetRetailerByIdAsync(int retailerId);
        Task UpdateRetailerAsync(int retailerId, UpdateRetailerViewModel updateRetailerViewModel);
        Task DeleteRetailerAsync(int retailerId);
    }
}
