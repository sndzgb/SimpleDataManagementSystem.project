using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public CategoriesRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<int> AddNewCategoryAsync(NewCategoryDTO newCategoryDTO)
        {
            if (newCategoryDTO == null)
            {
                throw new ArgumentNullException(nameof(newCategoryDTO));
            }
            
            var entity = await _dbContext.Categories.AddAsync(new CategoryEntity()
            {
                Name = newCategoryDTO.Name,
                Priority = newCategoryDTO.Priority
            });

            await _dbContext.SaveChangesAsync();

            return entity.Entity.ID;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);

            if (category == null)
            {
                return;
            }

            _dbContext.Remove(category);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<CategoriesDTO?> GetAllCategoriesAsync(int? take = 8, int? page = 1)
        {
            var total = await _dbContext.Categories.CountAsync();

            var categories = new CategoriesDTO();

            categories.PageInfo.Total = total;
            categories.PageInfo.Take = (int)take!;
            categories.PageInfo.Page = (int)page!;

            if (total > 0)
            {
                categories.Categories.AddRange
                (
                    await _dbContext.Categories
                        .OrderBy(x => x.ID)
                        .Skip((page!.Value - 1) * take!.Value)
                        .Take(take.Value)
                        .Select(s => new CategoryDTO()
                        {
                            Priority = s.Priority,
                            Name = s.Name,
                            ID = s.ID
                        })
                        .ToListAsync()
                );
            }

            return categories;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId)
        {
            var category = await _dbContext.Categories.Where(x => x.ID == categoryId).FirstOrDefaultAsync();

            if (category == null)
            {
                return null;
            }

            var categoryDTO = new CategoryDTO()
            {
                ID = category.ID,
                Name = category.Name,
                Priority = category.Priority
            };

            return categoryDTO;
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var entity = await _dbContext.Categories.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (entity == null)
            {
                return null;
            }

            var category = Map(entity);

            return category;
        }

        public async Task UpdateCategoryAsync(int categoryId, UpdateCategoryDTO updateCategoryDTO)
        {
            if (updateCategoryDTO == null)
            {
                return;
            }

            var category = await _dbContext.Categories.Where(x => x.ID == categoryId).FirstOrDefaultAsync();

            if (category == null)
            {
                return;
            }

            category.Name = updateCategoryDTO.Name;
            category.Priority = updateCategoryDTO.Priority;

            await _dbContext.SaveChangesAsync();
        }


        #region Mapping

        private Category? Map(CategoryEntity? entity)
        {
            if (entity == null)
            {
                return null;
            }

            var category = new Category()
            {
                ID = entity.ID,
                Name = entity.Name,
                Priority = entity.Priority
            };

            return category;
        }

        #endregion
    }
}
