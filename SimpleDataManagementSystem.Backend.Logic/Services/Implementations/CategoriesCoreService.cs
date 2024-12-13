using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class CategoriesCoreService : ICategoriesCoreService
    {
        private readonly ICategoriesCoreRepository _categoriesCoreRepository;


        public CategoriesCoreService(ICategoriesCoreRepository categoriesCoreRepository)
        {
            _categoriesCoreRepository = categoriesCoreRepository;
        }


        public async Task<CreateCategoryResponseDTO> CreateCategoryAsync(CreateCategoryRequestDTO createCategoryRequestDTO, CancellationToken cancellationToken)
        {
            var result = await _categoriesCoreRepository.CreateCategoryAsync(new CreateCategoryRequestDTO()
            {
                RequestedByUserId = createCategoryRequestDTO.RequestedByUserId,
                Name = createCategoryRequestDTO.Name,
                Priority = createCategoryRequestDTO.Priority
            }, cancellationToken);

            var response = new CreateCategoryResponseDTO()
            {
                Id = result.ID,
                Name = result.Name,
                Priority = result.Priority
            };

            return response;
        }

        public async Task DeleteCategoryAsync(DeleteCategoryRequestDTO deleteCategoryRequestDTO, CancellationToken cancellationToken)
        {
            await _categoriesCoreRepository.DeleteCategoryAsync(deleteCategoryRequestDTO, cancellationToken);
        }

        public async Task<GetCategoriesResponseDTO> GetCategoriesAsync(GetCategoriesRequestDTO getCategoriesRequestDTO, CancellationToken cancellationToken)
        {
            var categories = await _categoriesCoreRepository.GetCategoriesAsync(getCategoriesRequestDTO, cancellationToken);

            var response = new GetCategoriesResponseDTO();
            response.Categories = new List<GetCategoriesResponseDTO.CategoryDTO>();
            response.Categories.AddRange(categories.Categories.Select(x => new GetCategoriesResponseDTO.CategoryDTO()
            {
                Id = x.ID,
                Name = x.Name,
                Priority = x.Priority
            }));

            response.PageInfo = new GetCategoriesResponseDTO.PageDTO()
            {
                Total = categories.PageInfo.Total
            };

            return response;
        }

        public async Task<GetCategoryResponseDTO?> GetCategoryAsync(GetCategoryRequestDTO getCategoryRequestDTO, CancellationToken cancellationToken)
        {
            var category = await _categoriesCoreRepository.GetCategoryAsync(getCategoryRequestDTO, cancellationToken);

            if (category == null)
            {
                return null;
            }

            var response = new GetCategoryResponseDTO()
            {
                Id = category.ID,
                Name = category.Name,
                Priority = category.Priority
            };

            return response;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryRequestDTO updateCategoryRequestDTO, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(updateCategoryRequestDTO);

            var existingCategory = await _categoriesCoreRepository.GetCategoryAsync(new GetCategoryRequestDTO()
            {
                CategoryId = updateCategoryRequestDTO.RequestMetadata.CategoryId
            }, cancellationToken);

            if (existingCategory == null)
            {
                throw new RequiredRecordNotFoundException();
            }

            await _categoriesCoreRepository.UpdateCategoryAsync(updateCategoryRequestDTO, cancellationToken);
        }
    }
}
