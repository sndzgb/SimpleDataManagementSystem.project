using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
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


        public ItemsService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }


        public async Task<string> AddNewItemAsync(NewItemDTO newItemDTO)
        {
            return await _itemsRepository.AddNewItemAsync(newItemDTO);
        }

        public async Task DeleteItemAsync(string itemId)
        {
            await _itemsRepository.DeleteItemAsync(itemId);
        }

        public async Task<List<ItemDTO>> GetAllItemsAsync(int? take = 8, int? page = 1)
        {
            return await _itemsRepository.GetAllItemsAsync(take, page);
        }

        public async Task<ItemDTO?> GetItemByIdAsync(string itemId)
        {
            return await _itemsRepository.GetItemByIdAsync(itemId);
        }

        public async Task UpdateItemAsync(string itemId, UpdateItemDTO updateItemDTO)
        {
            await _itemsRepository.UpdateItemAsync(itemId, updateItemDTO);
        }
    }
}
