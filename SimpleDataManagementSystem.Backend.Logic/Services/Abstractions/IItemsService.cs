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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newItemDTO"></param>
        /// <exception cref="RecordExistsException"></exception>
        /// <returns></returns>
        Task<string> AddNewItemAsync(NewItemDTO newItemDTO);
        Task<ItemsDTO?> GetAllItemsAsync(int? take = 8, int? page = 1);
        Task<ItemDTO?> GetItemByIdAsync(string itemId);
        Task UpdateItemAsync(string itemId, UpdateItemDTO updateItemDTO);
        Task DeleteItemAsync(string itemId);
        Task UpdateItemPartialAsync(string itemId);
    }
}
