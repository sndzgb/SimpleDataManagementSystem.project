using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public interface IItemsService
    {
        Task<string> AddNewItemAsync(NewItemViewModel newItemViewModel);
        Task<ItemsViewModel> GetAllItemsAsync(int? take = 8, int? page = 1);
        Task<ItemViewModel> GetItemByIdAsync(string itemId);
        Task UpdateItemAsync(string itemId, UpdateItemViewModel updateItemViewModel);
        Task DeleteItemAsync(string itemId);
        Task UpdateItemPartialAsync(string itemId);
        Task<ItemsSearchResponseViewModel> SearchItemsAsync(ItemsSearchRequestViewModel itemsSearchRequestViewModel);
    }
}
