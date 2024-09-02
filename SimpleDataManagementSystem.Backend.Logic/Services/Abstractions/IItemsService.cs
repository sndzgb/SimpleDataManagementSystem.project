using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface IItemsService
    {
        Task<string> AddNewItemAsync(NewItemDTO newItemDTO);
        Task<List<ItemDTO>> GetAllItemsAsync(int? take = 8, int? page = 1);
        Task<ItemDTO?> GetItemByIdAsync(string itemId);
        Task UpdateItemAsync(string itemId, UpdateItemDTO updateItemDTO);
        Task DeleteItemAsync(string itemId);
    }
}
