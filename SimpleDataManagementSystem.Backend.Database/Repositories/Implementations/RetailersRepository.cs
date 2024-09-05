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
    public class RetailersRepository : IRetailersRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public RetailersRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public async Task<int> AddNewRetailerAsync(NewRetailerDTO newRetailerDTO)
        {
            if (newRetailerDTO == null)
            {
                throw new ArgumentNullException(nameof(newRetailerDTO));
            }

            var newRetailer = await _dbContext.Retailers.AddAsync(new RetailerEntity()
            {
                Name = newRetailerDTO.Name,
                Priority = newRetailerDTO.Priority,
                LogoImageUrl = newRetailerDTO.LogoImageUrl
            });

            await _dbContext.SaveChangesAsync();

            return newRetailer.Entity.ID;
        }

        public async Task DeleteRetailerAsync(int retailerId)
        {
            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

            if (retailer == null)
            {
                return;
            }

            _dbContext.Retailers.Remove(retailer);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<RetailersDTO?> GetAllRetailersAsync(int? take = 8, int? page = 1)
        {
            var total = await _dbContext.Retailers.CountAsync();

            var retailers = new RetailersDTO();

            retailers.PageInfo.Total = total;
            retailers.PageInfo.Take = (int)take!;
            retailers.PageInfo.Page = (int)page!;

            if (total > 0)
            {
                retailers.Retailers.AddRange
                (
                    await _dbContext.Retailers
                        .OrderBy(x => x.ID)
                        .Skip((page!.Value - 1) * take!.Value)
                        .Take(take.Value)
                        .Select(s => new RetailerDTO()
                        {
                            ID = s.ID,
                            Name = s.Name,
                            Priority = s.Priority,
                            LogoImageUrl = s.LogoImageUrl
                        })
                        .ToListAsync()
                );
            }

            return retailers;
        }

        public async Task<RetailerDTO?> GetRetailerByIdAsync(int retailerId)
        {
            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

            if (retailer == null)
            {
                return null;
            }

            return new RetailerDTO()
            {
                ID = retailer.ID,
                Name = retailer.Name,
                Priority = retailer.Priority,
                LogoImageUrl = retailer.LogoImageUrl
            };
        }

        public async Task UpdateRetailerAsync(int retailerId, UpdateRetailerDTO updateRetailerDTO)
        {
            if (updateRetailerDTO == null)
            {
                return;
            }

            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

            if (retailer == null) // if the retailer does not exist for some reason...
            {
                return;
            }

            retailer.Name = updateRetailerDTO.Name;
            retailer.Priority = updateRetailerDTO.Priority;

            if (!string.IsNullOrEmpty(updateRetailerDTO.LogoImageUrl))
            {
                retailer.LogoImageUrl = updateRetailerDTO.LogoImageUrl;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRetailerPartialAsync(int retailerId)
        {
            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

            if (retailer == null)
            {
                return;
            }

            retailer.LogoImageUrl = null;

            await _dbContext.SaveChangesAsync();
        }
    }
}
