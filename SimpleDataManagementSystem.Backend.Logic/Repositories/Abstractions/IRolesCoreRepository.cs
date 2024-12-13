using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IRolesCoreRepository
    {
        Task<List<Role>> GetAllRolesAsync(CancellationToken cancellationToken);
    }
}
