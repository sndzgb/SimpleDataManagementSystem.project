using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class GetUsersResponseDTO
    {
        public List<UserDTO> Users { get; set; }
        public PageDTO PageInfo { get; set; }

        public class PageDTO
        {
            public int Total { get; set; }
        }

        public class UserDTO
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
}
