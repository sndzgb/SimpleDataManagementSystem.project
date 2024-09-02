using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class UserLogInRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
