using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class RolesCoreService : IRolesCoreService
    {
        private readonly IRolesCoreRepository _rolesCoreRepository;


        public RolesCoreService(IRolesCoreRepository rolesCoreRepository)
        {
            _rolesCoreRepository = rolesCoreRepository;
        }


        public async Task<GetRolesResponseDTO> GetRolesAsync(CancellationToken cancellationToken)
        {
            var roles = await _rolesCoreRepository.GetAllRolesAsync(cancellationToken);

            var response = new GetRolesResponseDTO();
            response.Roles = new List<GetRolesResponseDTO.RoleDTO>();
            response.Roles.AddRange(roles.Select(x => new GetRolesResponseDTO.RoleDTO()
            {
                Id = x.ID,
                Name = x.Name
            }));

            return response;
        }
    }
}
