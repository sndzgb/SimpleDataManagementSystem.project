using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IItemsService
    {
        Task<CreateItemResponseViewModel> CreateItemAsync(
            CreateItemRequestViewModel createItemViewModel, CancellationToken cancellationToken
        );
        Task<GetMultipleItemsResponseViewModel> GetMultipleItemsAsync(
            CancellationToken cancellationToken, bool onlyEnabled = true, int? take = 8, int? page = 1
        );
        Task<GetSingleItemResponseViewModel> GetSingleItemAsync(string itemId, CancellationToken cancellationToken);
        Task UpdateItemAsync(string itemId, UpdateItemViewModel updateItemViewModel, CancellationToken cancellationToken);
        Task DeleteItemAsync(string itemId, CancellationToken cancellationToken);
        Task ToggleItemEnabledDisabledStatusAsync(string itemId, CancellationToken cancellationToken);
        Task<SearchItemsResponseViewModel> SearchItemsAsync(SearchItemsRequestViewModel itemsSearchRequestViewModel, CancellationToken cancellationToken);
        Task ToggleMonitoredItemAsync(string itemId, CancellationToken cancellationToken);
    }
}
