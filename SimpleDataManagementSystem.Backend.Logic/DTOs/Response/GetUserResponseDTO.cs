using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class GetUserResponseDTO
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
        public DateTime CreatedUtc { get; set; }
        public RoleDTO Role { get; set; }

        public class RoleDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
