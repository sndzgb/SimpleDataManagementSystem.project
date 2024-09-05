using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions
{
    public interface IRolesRepository
    {
        Task<List<RoleDTO>?> GetAllRolesAsync();
    }
}
