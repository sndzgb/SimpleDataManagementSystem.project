using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IItemsCoreRepository
    {
        Task<Item?> GetItemAsync(GetItemRequestDTO dto, CancellationToken cancellationToken);

        Task<ItemsDTO> GetItemsAsync(GetItemsRequestDTO dto, CancellationToken cancellationToken);
        Task<SearchItemsDTO> SearchItemsAsync(SearchItemsRequestDTO dto, CancellationToken cancellationToken);

        Task UpdateItemAsync(UpdateItemRequestDTO updateItemRequestDTO, CancellationToken cancellationToken);

        Task DeleteItemAsync(DeleteItemRequestDTO deleteItemRequestDTO, CancellationToken cancellationToken);

        Task<Item> CreateItemAsync(CreateItemRequestDTO createItemRequestDTO, CancellationToken cancellationToken);
        
        Task ToggleItemEnabledDisabledStatusAsync(
            ToggleItemEnabledDisabledStatusRequestDTO toggleItemEnabledDisabledStatusRequestDTO,
            CancellationToken cancellationToken
        );

        Task ToggleMonitoredItemAsync(ToggleMonitoredItemRequestDTO toggleMonitoredItemRequestDTO, CancellationToken cancellationToken);
    }
}
