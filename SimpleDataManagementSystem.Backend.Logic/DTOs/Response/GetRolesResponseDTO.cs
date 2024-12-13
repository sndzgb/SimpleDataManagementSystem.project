using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public class GetRolesResponseDTO
    {
        public List<RoleDTO> Roles { get; set; }

        public class RoleDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
