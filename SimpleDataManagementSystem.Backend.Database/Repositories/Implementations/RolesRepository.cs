using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class RolesRepository : IRolesRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public RolesRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _dbContext.Roles.Select(x => new RoleDTO()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return roles;
        }
    }
}
