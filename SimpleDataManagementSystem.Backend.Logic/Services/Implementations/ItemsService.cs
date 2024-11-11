using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
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
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IRetailersRepository _retailersRepository;


        public ItemsService(IItemsRepository itemsRepository, IRetailersRepository retailersRepository)
        {
            _itemsRepository = itemsRepository;
            _retailersRepository = retailersRepository;
        }


        public async Task<string> AddNewItemAsync(NewItemDTO newItemDTO)
        {
            if (newItemDTO == null)
            {
                throw new ArgumentNullException(nameof(newItemDTO));
            }

            var items = await _itemsRepository.GetItemsByTitleAsync(newItemDTO.Nazivproizvoda);

            if (items != null)
            {
                if (items.Any(x => x.Nazivproizvoda == newItemDTO.Nazivproizvoda || x.Retailer.ID == newItemDTO.RetailerId))
                {
                    throw new RecordExistsException($@"Item with name '{newItemDTO.Nazivproizvoda}' 
                                                        on the specified retailer already exists.");
                }
            }

            #region old - composite key
            //if (items != null) 
            //{
            //    // check if item with "title" and "retailerId" already exist
            //    foreach (var item in items)
            //    {
            //        if (item.Retailer.ID == newItemDTO.RetailerId)
            //        {
            //            throw new RecordExistsException($"Item with name '{newItemDTO.Nazivproizvoda}' already exists.");
            //        }
            //    }
            //}
            #endregion

            var retailer = await _retailersRepository.GetRetailerByIdAsync(newItemDTO.RetailerId);

            if (retailer == null)
            {
                throw new RequiredRecordNotFoundException("Retailer was not found.");
            }

            return await _itemsRepository.AddNewItemAsync(newItemDTO);
        }

        public async Task DeleteItemAsync(string itemId)
        {
            await _itemsRepository.DeleteItemAsync(itemId);
        }

        public async Task<ItemsDTO?> GetAllItemsAsync(int? take = 8, int? page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (take < 1)
            {
                take = 8;
            }

            var items = await _itemsRepository.GetAllItemsAsync(take, page);
            
            return items;
        }

        public async Task<ItemDTO?> GetItemByIdAsync(string itemId)
        {
            return await _itemsRepository.GetItemByIdAsync(itemId);
        }

        public async Task UpdateItemAsync(string itemId, UpdateItemDTO updateItemDTO)
        {
            if (updateItemDTO == null)
            {
                return;
            }

            await _itemsRepository.UpdateItemAsync(itemId, updateItemDTO);
        }

        public async Task UpdateItemPartialAsync(string itemId)
        {
            await _itemsRepository.UpdateItemPartialAsync(itemId);
        }

        public async Task<ItemSearchResponseDTO> SearchItemsAsync(ItemSearchRequestDTO request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                return new ItemSearchResponseDTO()
                {
                    Request = request
                };
            }

            var results = await _itemsRepository.SearchItemsAsync(request, cancellationToken);

            var dto = new ItemSearchResponseDTO()
            {
                PageInfo = new PagedDTO()
                {
                    Page = request.Page,
                    Take = request.Take,
                    Total = results.Item2,
                },
                Request = request
            };

            if (results.Item1 != null)
            {
                dto.Items = results.Item1.Select(x => new ItemDTO()
                {
                    Cijena = x.Cijena,
                    Nazivproizvoda = x.Nazivproizvoda,
                    URLdoslike = x.URLdoslike
                }).ToList();
            }

            return dto;
        }
    }
}
