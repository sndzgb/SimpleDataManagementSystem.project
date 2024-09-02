using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Write
{
    public class UpdateUserDTO
    {
        public int RoleId { get; set; }
        public string Username { get; set; }
    }
}
