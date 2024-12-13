using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IItemsCoreService
    {
        Task<GetItemResponseDTO?> GetItemAsync(GetItemRequestDTO getItemRequestDTO, CancellationToken cancellationToken);
        Task<GetItemsResponseDTO?> GetItemsAsync(GetItemsRequestDTO getItemsRequestDTO, CancellationToken cancellationToken);
        
        Task<SearchItemsResponseDTO> SearchItemsAsync(SearchItemsRequestDTO searchItemsRequestDTO, CancellationToken cancellationToken);
        
        Task UpdateItemAsync(UpdateItemRequestDTO updateItemRequestDTO, CancellationToken cancellationToken);

        Task DeleteItemAsync(DeleteItemRequestDTO deleteItemRequestDTO, CancellationToken cancellationToken);

        Task<CreateItemResponseDTO> CreateItemAsync(CreateItemRequestDTO createItemRequestDTO, CancellationToken cancellationToken);

        Task ToggleMonitoredItemAsync(ToggleMonitoredItemRequestDTO toggleMonitoredItemRequestDTO, CancellationToken cancellationToken);

        Task ToggleItemEnabledDisabledStatusAsync(
            ToggleItemEnabledDisabledStatusRequestDTO toggleItemEnabledDisabledStatusRequestDTO, CancellationToken cancellationToken
        );
    }
}
