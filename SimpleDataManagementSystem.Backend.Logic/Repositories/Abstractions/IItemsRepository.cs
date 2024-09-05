using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IItemsRepository
    {
        Task DeleteItemAsync(string itemId);
        Task<string> AddNewItemAsync(NewItemDTO newItemDTO);
        Task UpdateItemAsync(string itemId, UpdateItemDTO updateItemDTO);
        Task<ItemsDTO?> GetAllItemsAsync(int? take = 8, int? page = 1);
        Task<List<Item>?> GetItemsByTitleAsync(string title);
        Task<ItemDTO?> GetItemByIdAsync(string itemId);
        Task UpdateItemPartialAsync(string itemId);
    }
}
