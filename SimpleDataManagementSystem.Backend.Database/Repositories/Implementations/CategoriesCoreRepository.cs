using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class CategoriesCoreRepository : ICategoriesCoreRepository
    {
        private readonly IDbContextFactory<SimpleDataManagementSystemDbContext> _dbContextFactory;
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public CategoriesCoreRepository(
                IDbContextFactory<SimpleDataManagementSystemDbContext> dbContextFactory,
                SimpleDataManagementSystemDbContext dbContext
            )
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContext;
        }


        public async Task<Category> CreateCategoryAsync(CreateCategoryRequestDTO createCategoryRequestDTO, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Categories.AddAsync(new Entities.CategoryEntity()
            {
                Name = createCategoryRequestDTO.Name,
                Priority = createCategoryRequestDTO.Priority
            }, cancellationToken);

            var newCategory = new Category()
            {
                ID = result.Entity.ID,
                Name = result.Entity.Name,
                Priority = result.Entity.Priority
            };

            await _dbContext.SaveChangesAsync(cancellationToken);

            return newCategory;
        }

        public async Task DeleteCategoryAsync(DeleteCategoryRequestDTO deleteCategoryRequestDTO, CancellationToken cancellationToken)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(deleteCategoryRequestDTO.Metadata.CategoryId, cancellationToken);

            if (existingCategory == null)
            {
                return;
            }

            _dbContext.Remove(existingCategory);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

		public async Task<CategoriesDTO> GetCategoriesAsync(GetCategoriesRequestDTO getCategoriesRequestDTO, CancellationToken cancellationToken)
        {
            using var categoriesDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            using var countDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

			var categoriesQueryable = categoriesDbContext
				.Categories
				.Skip((getCategoriesRequestDTO.PageInfo.Page - 1) * getCategoriesRequestDTO.PageInfo.Take)
				.Take(getCategoriesRequestDTO.PageInfo.Take)
                .OrderBy(x => x.Name);

            var countQueryable = countDbContext.Categories;


			//var categoriesQueryable = (await _dbContextFactory.CreateDbContextAsync(cancellationToken)).Categories
   //                                         .Skip((getCategoriesRequestDTO.PageInfo.Page - 1) * getCategoriesRequestDTO.PageInfo.Take)
   //                                         .Take(getCategoriesRequestDTO.PageInfo.Take);

   //         var countQueryable = (await _dbContextFactory.CreateDbContextAsync(cancellationToken)).Categories;

            var countTask = countQueryable.CountAsync(cancellationToken);
            var categoriesTask = categoriesQueryable.ToListAsync(cancellationToken);

            await Task.WhenAll(countTask, categoriesTask);

            var countTaskResult = await countTask;
            var categoriesTaskResult = await categoriesTask;

            var response = new CategoriesDTO()
            {
                Categories = categoriesTaskResult.Select(x => new Category()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Priority = x.Priority
                }).ToList(),
                PageInfo = new CategoriesDTO.PageDTO()
                {
                    Total = countTaskResult
                }
            };

            return response;

            //var categories = await _dbContext.Categories
            //    .Skip((getCategoriesRequestDTO.PageInfo.Page - 1) * getCategoriesRequestDTO.PageInfo.Take)
            //    .Take(getCategoriesRequestDTO.PageInfo.Take)
            //    .ToListAsync();

            //var result = categories.Select(x => new Category()
            //{
            //    ID = x.ID,
            //    Name = x.Name,
            //    Priority = x.Priority
            //}).ToList();

            //return result;
        }

        public async Task<Category?> GetCategoryAsync(GetCategoryRequestDTO getCategoryRequestDTO, CancellationToken cancellationToken)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(getCategoryRequestDTO.CategoryId, cancellationToken);

            if (existingCategory == null) 
            {
                return null;
            }

            var result = new Category()
            {
                ID = existingCategory.ID,
                Name = existingCategory.Name,
                Priority = existingCategory.Priority
            };

            return result;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryRequestDTO updateCategoryRequestDTO, CancellationToken cancellationToken)
        {
            var existingCategory = await _dbContext.Categories
                                                        .FindAsync(
                                                            updateCategoryRequestDTO.RequestMetadata.CategoryId, 
                                                            cancellationToken
                                                        );

            if (existingCategory == null)
            {
                return;
            }

            existingCategory.Priority = updateCategoryRequestDTO.Priority;
            existingCategory.Name = updateCategoryRequestDTO.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
	}
}
