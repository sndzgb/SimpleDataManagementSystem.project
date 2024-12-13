using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SimpleDataManagementSystem.Backend.Database.Entities;
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
    public class RetailersCoreRepository : IRetailersCoreRepository
    {
        private readonly IDbContextFactory<SimpleDataManagementSystemDbContext> _dbContextFactory;
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public RetailersCoreRepository(
            IDbContextFactory<SimpleDataManagementSystemDbContext> dbContextFactory,
                SimpleDataManagementSystemDbContext dbContext
            )
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContext;
        }


        public async Task<Retailer> CreateRetailerAsync(CreateRetailerRequestDTO createRetailerRequestDTO, CancellationToken cancellationToken)
        {
            var newRetailer = new RetailerEntity();
            newRetailer.Name = createRetailerRequestDTO.Name;
            newRetailer.Priority = createRetailerRequestDTO.Priority;
            newRetailer.LogoImageUrl = createRetailerRequestDTO.LogoImageUrl;

            var createdEntity = await _dbContext.Retailers.AddAsync(newRetailer, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var result = new Retailer()
            {
                ID = createdEntity.Entity.ID,
                LogoImageUrl = createdEntity.Entity.LogoImageUrl,
                Name = createdEntity.Entity.Name,
                Priority = createdEntity.Entity.Priority
            };

            return result;
        }

        public async Task DeleteRetailerAsync(DeleteRetailerRequestDTO deleteRetailerRequestDTO, CancellationToken cancellationToken)
        {
            var existingRetailer = await _dbContext
                                                .Retailers
                                                .Where(x => x.ID == deleteRetailerRequestDTO.Metadata.RetailerId)
                                                .FirstOrDefaultAsync(cancellationToken);

            if (existingRetailer == null)
            {
                return;
            }

            _dbContext.Remove(existingRetailer);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Retailer?> GetRetailerAsync(GetRetailerRequestDTO getRetailerRequestDTO, CancellationToken cancellationToken)
        {
            var retailer = await _dbContext
                                        .Retailers
                                        .Where(x => x.ID == getRetailerRequestDTO.RetailerId)
                                        .FirstOrDefaultAsync(cancellationToken);

            if (retailer == null)
            {
                return null;
            }

            var model = new Retailer()
            {
                ID = retailer.ID,
                LogoImageUrl = retailer.LogoImageUrl,
                Name = retailer.Name,
                Priority = retailer.Priority
            };

            return model;
        }

        public async Task<RetailersDTO> GetRetailersAsync(GetRetailersRequestDTO getRetailersRequestDTO, CancellationToken cancellationToken)
        {
            using var retailersDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var retailersQueryable = retailersDbContext.Retailers
									.Skip((getRetailersRequestDTO.PageInfo.Page - 1) * getRetailersRequestDTO.PageInfo.Take)
									.Take(getRetailersRequestDTO.PageInfo.Take)
                                    .OrderBy(x => x.Name);
            //var retailersQueryable = (await _dbContextFactory.CreateDbContextAsync(cancellationToken)).Retailers
            //                                .Skip((getRetailersRequestDTO.PageInfo.Page - 1) * getRetailersRequestDTO.PageInfo.Take)
            //                                .Take(getRetailersRequestDTO.PageInfo.Take);

            using var countDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var countQueryable = countDbContext.Retailers;
			//var countQueryable = (await _dbContextFactory.CreateDbContextAsync(cancellationToken)).Retailers;

            var countTask = countQueryable.CountAsync(cancellationToken);
            var retailersTask = retailersQueryable.ToListAsync(cancellationToken);

            await Task.WhenAll(countTask, retailersTask);

            var countTaskResult = await countTask;
            var retailersTaskResult = await retailersTask;

            var response = new RetailersDTO()
            {
                Retailers = retailersTaskResult.Select(x => new Retailer()
                {
                    ID = x.ID,
                    LogoImageUrl = x.LogoImageUrl,
                    Name = x.Name,
                    Priority = x.Priority
                }).ToList(),
                PageInfo = new RetailersDTO.PageDTO()
                {
                    Total = countTaskResult
                }
            };

            return response;

            //var queryable = _dbContext.Retailers;

            //var retailersQueryable = queryable
            //                                .Skip((getRetailersRequestDTO.PageInfo.Page - 1) * getRetailersRequestDTO.PageInfo.Take)
            //                                .Take(getRetailersRequestDTO.PageInfo.Take);

            //var countQueryable = queryable;

            //var countTask = countQueryable.CountAsync(cancellationToken);
            //var retailersTask = retailersQueryable.ToListAsync(cancellationToken);

            //await Task.WhenAll(countTask, retailersTask);

            //var countTaskResult = await countTask;
            //var retailersTaskResult = await retailersTask;

            //var response = new RetailersDTO()
            //{
            //    PageInfo = new RetailersDTO.PageDTO()
            //    {
            //        Total = countTaskResult
            //    },
            //    Retailers = retailersTaskResult.Select(x => new Retailer()
            //    {
            //        ID = x.ID,
            //        LogoImageUrl = x.LogoImageUrl,
            //        Name = x.Name,
            //        Priority = x.Priority
            //    }).ToList()
            //};

            //return response;






            //var retailers = await _dbContext
            //                            .Retailers
            //                            .Skip((getRetailersRequestDTO.Request.Page - 1) * getRetailersRequestDTO.Request.Take)
            //                            .Take(getRetailersRequestDTO.Request.Take)
            //                            .ToListAsync(cancellationToken);

            //if (retailers == null)
            //{
            //    return null;
            //}

            //var model = new List<Retailer>();
            //model.AddRange
            //(
            //    retailers.Select
            //    (
            //        x => new Retailer()
            //        {
            //            ID = x.ID,
            //            LogoImageUrl = x.LogoImageUrl,
            //            Name = x.Name,
            //            Priority = x.Priority
            //        }
            //    )
            //);



            //return model.AsEnumerable();
        }

        public async Task UpdateRetailerAsync(UpdateRetailerRequestDTO updateRetailerRequestDTO, CancellationToken cancellationToken)
        {
            using var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            
            var existingRetailer = await db
                                                .Retailers
                                                .Where(x => x.ID == updateRetailerRequestDTO.RequestMetadata.RetailerId)
                                                .FirstOrDefaultAsync(cancellationToken);

            if (existingRetailer == null)
            {
                return;
            }

            existingRetailer.Priority = updateRetailerRequestDTO.Priority;
            existingRetailer.Name = updateRetailerRequestDTO.Name;

            if (updateRetailerRequestDTO.DeleteCurrentLogoImage)
            {
                existingRetailer.LogoImageUrl = null;
            }

            if (!string.IsNullOrEmpty(updateRetailerRequestDTO.LogoImageUrl))
            {
                existingRetailer.LogoImageUrl = updateRetailerRequestDTO.LogoImageUrl;
            }

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
