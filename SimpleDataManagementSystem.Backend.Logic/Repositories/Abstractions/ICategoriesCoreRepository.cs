using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface ICategoriesCoreRepository
    {
        Task<Category?> GetCategoryAsync(GetCategoryRequestDTO getCategoryRequestDTO, CancellationToken cancellationToken);
        Task<CategoriesDTO> GetCategoriesAsync(
            GetCategoriesRequestDTO getMultipleCategoriesRequestDTO,
            CancellationToken cancellationToken
        );

        Task<Category> CreateCategoryAsync(
            CreateCategoryRequestDTO createCategoryRequestDTO,
            CancellationToken cancellationToken
        );

        Task UpdateCategoryAsync(UpdateCategoryRequestDTO updateCategoryRequestDTO, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(DeleteCategoryRequestDTO deleteCategoryRequestDTO, CancellationToken cancellationToken);
    }
}
