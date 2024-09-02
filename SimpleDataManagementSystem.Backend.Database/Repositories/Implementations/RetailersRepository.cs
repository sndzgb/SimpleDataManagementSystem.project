using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
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
            var newRetailer = await _dbContext.Retailers.AddAsync(new Entities.RetailerEntity()
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
            if (retailerId == null)
            {
                return;
            }

            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

            if (retailer == null)
            {
                return;
            }

            _dbContext.Retailers.Remove(retailer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<RetailerDTO>> GetAllRetailersAsync(int? take = 8, int? page = 1)
        {
            return await _dbContext.Retailers.OrderBy(x => x.ID)
                .Skip((page!.Value - 1) * take!.Value)
                .Take(take.Value)
                .Select(s => new RetailerDTO()
                {
                    ID = s.ID,
                    Name = s.Name,
                    LogoImageUrl = s.LogoImageUrl
                })
                .ToListAsync();
        }

        public async Task<RetailerDTO?> GetRetailerByIdAsync(int retailerId)
        {
            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

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
            var retailer = await _dbContext.Retailers.FindAsync(retailerId);

            if (retailer == null)
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
    }
}
