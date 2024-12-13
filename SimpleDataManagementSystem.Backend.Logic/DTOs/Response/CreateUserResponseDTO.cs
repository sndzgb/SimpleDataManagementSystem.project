using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class CreateUserResponseDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Roles Role { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
    }
}
