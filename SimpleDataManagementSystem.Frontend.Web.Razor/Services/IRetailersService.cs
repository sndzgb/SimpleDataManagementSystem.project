using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IRetailersService
    {
        Task<int> CreateRetailerAsync(CreateRetailerViewModel createRetailerViewModel, CancellationToken cancellationToken);
        Task<GetMultipleRetailersResponseViewModel> GetMultipleRetailersAsync(
            CancellationToken cancellationToken, int? take = 8, int? page = 1
        );
        Task<GetSingleRetailerResponseViewModel> GetSingleRetailerAsync(int retailerId, CancellationToken cancellationToken);
        Task UpdateRetailerAsync(int retailerId, UpdateRetailerViewModel updateRetailerViewModel, CancellationToken cancellationToken);
        Task DeleteRetailerAsync(int retailerId, CancellationToken cancellationToken);
    }
}
