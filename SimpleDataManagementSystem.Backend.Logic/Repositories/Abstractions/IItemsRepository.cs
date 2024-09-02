﻿using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
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
        Task<List<ItemDTO>> GetAllItemsAsync(int? take = 8, int? page = 1);
        Task<ItemDTO?> GetItemByIdAsync(string itemId);
    }
}
