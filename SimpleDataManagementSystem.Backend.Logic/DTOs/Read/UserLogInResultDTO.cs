using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class UserLogInResultDTO
    {
        public List<MyClaim> Claims { get; set; }
        public string Username { get; set; }

        public class MyClaim
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
