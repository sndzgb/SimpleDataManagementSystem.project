using Microsoft.Extensions.DependencyInjection;
using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface ICategoriesCoreService
    {
        Task<GetCategoryResponseDTO?> GetCategoryAsync(GetCategoryRequestDTO getCategoryRequestDTO, CancellationToken cancellationToken);
        Task<GetCategoriesResponseDTO> GetCategoriesAsync(
            GetCategoriesRequestDTO getMultipleCategoriesRequestDTO, 
            CancellationToken cancellationToken
        );
        
        Task<CreateCategoryResponseDTO> CreateCategoryAsync(
            CreateCategoryRequestDTO createCategoryRequestDTO, 
            CancellationToken cancellationToken
        );

        Task UpdateCategoryAsync(UpdateCategoryRequestDTO updateCategoryRequestDTO, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(DeleteCategoryRequestDTO deleteCategoryRequestDTO, CancellationToken cancellationToken);
    }
}
