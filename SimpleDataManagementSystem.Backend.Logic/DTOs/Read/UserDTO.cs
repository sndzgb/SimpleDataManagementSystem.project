using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
