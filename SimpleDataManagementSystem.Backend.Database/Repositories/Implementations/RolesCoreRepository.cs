using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class RolesCoreRepository : IRolesCoreRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public RolesCoreRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Role>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _dbContext.Roles.Select(x => new Role()
            {
                ID = x.Id,
                Name = x.Name
            }).ToListAsync(cancellationToken);

            return roles;
        }
    }
}
