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
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;


        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }


        public async Task<int> AddNewCategoryAsync(NewCategoryDTO newCategoryDTO)
        {
            return await _categoriesRepository.AddNewCategoryAsync(newCategoryDTO);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _categoriesRepository.DeleteCategoryAsync(categoryId);
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync(int? take = 8, int? page = 1)
        {
            return await _categoriesRepository.GetAllCategoriesAsync(take, page);
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoriesRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task UpdateCategoryAsync(int categoryId, UpdateCategoryDTO updateCategoryDTO)
        {
            await _categoriesRepository.UpdateCategoryAsync(categoryId, updateCategoryDTO);
        }
    }
}
