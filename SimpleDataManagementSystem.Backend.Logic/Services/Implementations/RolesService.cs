using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _rolesRepository;


        public RolesService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }


        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            return await _rolesRepository.GetAllRolesAsync();
        }
    }
}
