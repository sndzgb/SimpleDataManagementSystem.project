using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class GetUserByLoginCredentialsResponseDTO
    {
        public string[] Roles { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
    }
}
