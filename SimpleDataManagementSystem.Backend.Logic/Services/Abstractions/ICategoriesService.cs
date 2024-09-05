using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface ICategoriesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newCategoryDTO"></param>
        /// <exception cref="RecordExistsException"></exception>
        /// <returns></returns>
        Task<int> AddNewCategoryAsync(NewCategoryDTO newCategoryDTO);
        Task<CategoriesDTO?> GetAllCategoriesAsync(int? take = 8, int? page = 1);
        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId);
        Task UpdateCategoryAsync(int categoryId, UpdateCategoryDTO updateCategoryDTO);
        Task DeleteCategoryAsync(int categoryId);
    }
}
