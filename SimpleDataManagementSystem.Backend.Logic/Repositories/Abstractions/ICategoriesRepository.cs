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
    public interface ICategoriesRepository
    {
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<int> AddNewCategoryAsync(NewCategoryDTO newCategoryDTO);
        Task<CategoriesDTO?> GetAllCategoriesAsync(int? take = 8, int? page = 1);
        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId);
        Task UpdateCategoryAsync(int categoryId, UpdateCategoryDTO updateCategoryDTO);
        Task DeleteCategoryAsync(int categoryId);
    }
}
