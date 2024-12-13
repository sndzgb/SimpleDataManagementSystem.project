using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class GetUserByLoginCredentialsRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
