using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class ItemsCoreService : IItemsCoreService
    {
        private readonly IItemsCoreRepository _itemsCoreRepository;
        private readonly INotificationsService _notificationsService;


        public ItemsCoreService(
                IItemsCoreRepository itemsCoreRepository,
                INotificationsService notificationsService
            )
        {
            _itemsCoreRepository = itemsCoreRepository;
            _notificationsService = notificationsService;
        }


        public async Task UpdateItemAsync(UpdateItemRequestDTO updateItemRequestDTO, CancellationToken cancellationToken)
        {
            if (updateItemRequestDTO == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(updateItemRequestDTO));
            }

            var existingItem = await _itemsCoreRepository.GetItemAsync(new GetItemRequestDTO()
            {
                IncludeCategory = true,
                IncludeRetailer = true,
                ItemId = updateItemRequestDTO.RequestMetadata.ItemId,
                Monitoring = new GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = true
                },
                RequestedByUserId = updateItemRequestDTO.RequestedByUserId
            }, cancellationToken);

            if (existingItem == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            await _itemsCoreRepository.UpdateItemAsync(updateItemRequestDTO, cancellationToken);
        }

        public async Task<CreateItemResponseDTO> CreateItemAsync(CreateItemRequestDTO createItemRequestDTO, CancellationToken cancellationToken)
        {
            var existingItemName = await _itemsCoreRepository.GetItemAsync(new GetItemRequestDTO()
            {
                IncludeCategory = false,
                IncludeRetailer = false,
                Monitoring = new GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = false
                },
                ItemId = createItemRequestDTO.Nazivproizvoda,
                RequestedByUserId = createItemRequestDTO.RequestedByUserId
            }, cancellationToken);

            if (existingItemName != null)
            {
                throw new RecordExistsException();
            }

            var createdItem = await _itemsCoreRepository.CreateItemAsync(new CreateItemRequestDTO()
            {
                Cijena = createItemRequestDTO.Cijena,
                RetailerId = createItemRequestDTO.RetailerId,
                Kategorija = createItemRequestDTO.Kategorija,
                URLdoslike = createItemRequestDTO.URLdoslike,
                Opis = createItemRequestDTO.Opis,
                Datumakcije = createItemRequestDTO.Datumakcije,
                IsEnabled = createItemRequestDTO.IsEnabled,
                Nazivproizvoda = createItemRequestDTO.Nazivproizvoda,
                RequestedByUserId = createItemRequestDTO.RequestedByUserId
            }, cancellationToken);

            var result = new CreateItemResponseDTO()
            {
                Cijena = createItemRequestDTO.Cijena,
                Category = new CreateItemResponseDTO.CategoryDTO()
                {
                    Id = createdItem.Category.ID,
                    Name = createdItem.Category.Name,
                    Priority = createdItem.Category.Priority
                },
                Retailer = new CreateItemResponseDTO.RetailerDTO()
                {
                    Priority = createdItem.Retailer.Priority,
                    Name = createdItem.Retailer.Name,
                    Id = createdItem.Retailer.ID,
                    LogoImageUrl = string.IsNullOrEmpty(createdItem.Retailer.LogoImageUrl) ? null : createdItem.Retailer.LogoImageUrl
                },
                URLdoslike = string.IsNullOrEmpty(createItemRequestDTO.URLdoslike) ? null : createdItem.URLdoslike,
                Opis = createItemRequestDTO.Opis,
                Datumakcije = createItemRequestDTO.Datumakcije,
                IsEnabled = createItemRequestDTO.IsEnabled,
                Nazivproizvoda = createItemRequestDTO.Nazivproizvoda
            };

            return result;
        }

        public async Task DeleteItemAsync(DeleteItemRequestDTO deleteItemRequestDTO, CancellationToken cancellationToken)
        {
            await _itemsCoreRepository.DeleteItemAsync(deleteItemRequestDTO, cancellationToken);
        }

        public async Task<GetItemResponseDTO?> GetItemAsync(GetItemRequestDTO getItemRequestDTO, CancellationToken cancellationToken)
        {
            var item = await _itemsCoreRepository.GetItemAsync(getItemRequestDTO, cancellationToken);

            if (item == null)
            {
                return null;
            }

            var response = new GetItemResponseDTO()
            {
                Cijena = item.Cijena,
                Datumakcije = item.Datumakcije,
                IsEnabled = item.IsEnabled,
                Nazivproizvoda = item.Nazivproizvoda,
                Opis = item.Opis,
                URLdoslike = item.URLdoslike
            };

            response.Category = new GetItemResponseDTO.CategoryDTO()
            {
                Id = item.Category.ID,
                Priority = item.Category.Priority,
                Name = item.Category.Name
            };

            response.Retailer = new GetItemResponseDTO.RetailerDTO()
            {
                Id = item.Retailer.ID,
                Name = item.Retailer.Name
            };

            response.Monitoring = new GetItemResponseDTO.MonitoredItemDTO()
            {
                IsMonitoredByUser = item
                                    .Monitored
                                    .Where(x => x.User.ID == getItemRequestDTO.RequestedByUserId)
                                    .FirstOrDefault()
                                    != null,
                StartedMonitoringAtUtc = item.
                                        Monitored.Where(x => x.User.ID == getItemRequestDTO.RequestedByUserId)
                                        .FirstOrDefault()?
                                        .StartedMonitoringAtUtc,
                TotalUsersMonitoringThisItem = item.Monitored.Count
            };

            return response;
        }

        public async Task<GetItemsResponseDTO?> GetItemsAsync(GetItemsRequestDTO getItemsRequestDTO, CancellationToken cancellationToken)
        {
            var items = await _itemsCoreRepository.GetItemsAsync(getItemsRequestDTO, cancellationToken);

            if (items.Items == null)
            {
                return null;
            }

            var response = new GetItemsResponseDTO();
            response.Items = new List<GetItemsResponseDTO.ItemDTO>();

            response.Items.AddRange(items.Items.Select(x => new GetItemsResponseDTO.ItemDTO()
            {
                Category = new GetItemsResponseDTO.ItemDTO.CategoryDTO()
                {
                    Id = x.Category.ID,
                    Name = x.Category.Name
                },
                Monitoring = new GetItemsResponseDTO.ItemDTO.MonitoredItem()
                {
                    IsMonitoredByCurrentUser = x.Monitored.Where(x => x.User.ID == getItemsRequestDTO.RequestedByUserId).FirstOrDefault() != null,
                    TotalUsersMonitoringThisItem = x.Monitored.Count()
                },
                Cijena = x.Cijena,
                Datumakcije = x.Datumakcije,
                IsEnabled = x.IsEnabled,
                //MonitoredByCurrentUser = (x.Monitored?.FirstOrDefault() != null) && (x.Monitored?.Count > 0),
                Nazivproizvoda = x.Nazivproizvoda,
                Opis = x.Opis,
                Retailer = new GetItemsResponseDTO.ItemDTO.RetailerDTO()
                {
                    Id = x.Retailer.ID,
                    Name = x.Retailer.Name,
                    Priority = x.Retailer.Priority
                },
                URLdoslike = x.URLdoslike
            }));

            response.PageInfo = new GetItemsResponseDTO.PageDTO()
            {
                Total = items.PageInfo.Total
            };

            return response;
        }

        public async Task<SearchItemsResponseDTO> SearchItemsAsync(SearchItemsRequestDTO searchItemsRequestDTO, CancellationToken cancellationToken)
        {
            if (searchItemsRequestDTO?.PageInfo?.Query == null)
            {
                throw new ArgumentNullException(nameof(searchItemsRequestDTO.PageInfo.Query), "Search query parameter is required.");
            }

            var result = await _itemsCoreRepository.SearchItemsAsync(searchItemsRequestDTO, cancellationToken);

            var response = new SearchItemsResponseDTO();
            response.Items = new List<SearchItemsResponseDTO.ItemDTO>();

            if (result.Items != null && result.Items.Count > 0)
            {
                response.Items.AddRange(result.Items.Select(x => new SearchItemsResponseDTO.ItemDTO()
                {
                    Cijena = x.Cijena,
                    Datumakcije = x.Datumakcije,
                    IsEnabled = x.IsEnabled,
                    Nazivproizvoda = x.Nazivproizvoda,
                    Opis = x.Opis,
                    URLdoslike = x.URLdoslike,
                    IsMonitoredByCurrentUser = x.Monitored == null ? false : true,
                    Category = new SearchItemsResponseDTO.ItemDTO.CategoryDTO()
                    {
                        Id = x.Category.ID
                    },
                    Retailer = new SearchItemsResponseDTO.ItemDTO.RetailerDTO()
                    {
                        Id = x.Retailer.ID
                    },
                    TotalUsersMonitoringThisItem = x.Monitored == null ? 0 : x.Monitored.Count()
                }));
            }

            response.PageInfo = new SearchItemsResponseDTO.PageDTO();
            response.PageInfo.Total = result.PageInfo.Total;

            return response;
        }

        public async Task ToggleItemEnabledDisabledStatusAsync(
                ToggleItemEnabledDisabledStatusRequestDTO toggleItemEnabledDisabledStatusRequestDTO,
                CancellationToken cancellationToken
            )
        {
            if (toggleItemEnabledDisabledStatusRequestDTO == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(toggleItemEnabledDisabledStatusRequestDTO));
            }

            if (toggleItemEnabledDisabledStatusRequestDTO!.Metadata == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(toggleItemEnabledDisabledStatusRequestDTO.Metadata));
            }

            var existingItem = await _itemsCoreRepository.GetItemAsync(new GetItemRequestDTO()
            {
                IncludeCategory = true,
                IncludeRetailer = true,
                ItemId = toggleItemEnabledDisabledStatusRequestDTO.Metadata.ItemId,
                Monitoring = new GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = false
                },
                RequestedByUserId = toggleItemEnabledDisabledStatusRequestDTO.RequestedByUserId
            }, cancellationToken);

            if (existingItem == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            await _itemsCoreRepository.ToggleItemEnabledDisabledStatusAsync(toggleItemEnabledDisabledStatusRequestDTO, cancellationToken);

            await _notificationsService.ItemUpdatedNotifierService.SendItemUpdatedNotificationAsync(
                $"Item '{toggleItemEnabledDisabledStatusRequestDTO.Metadata.ItemId}' status has been changed.",
                cancellationToken,
                new List<int>() { toggleItemEnabledDisabledStatusRequestDTO.RequestedByUserId },
                null
            );
        }

        public async Task ToggleMonitoredItemAsync(
                ToggleMonitoredItemRequestDTO toggleMonitoredItemRequestDTO,
                CancellationToken cancellationToken
            )
        {
            var item = await _itemsCoreRepository.GetItemAsync(new GetItemRequestDTO()
            {
                ItemId = toggleMonitoredItemRequestDTO.RequestMetadata.ItemId,
                IncludeCategory = false,
                IncludeRetailer = false,
                Monitoring = new GetItemRequestDTO.MonitoredItemDTO()
                {
                    IncludeMonitoring = true
                },
                RequestedByUserId = toggleMonitoredItemRequestDTO.RequestedByUserId
            }, cancellationToken);

            if (item == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            if (!item.IsEnabled)
            {
                throw new NotAllowedException("Disabled items cannot be monitored.");
            }

            await _itemsCoreRepository.ToggleMonitoredItemAsync(
                toggleMonitoredItemRequestDTO,
                cancellationToken
            );
        }
    }
}
