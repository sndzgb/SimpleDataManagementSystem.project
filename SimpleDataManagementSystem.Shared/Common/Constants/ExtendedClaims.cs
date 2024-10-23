using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Common.Constants
{
    public static class ExtendedClaims
    {
        public class Type
        {
            public const string UserId = "UserId";
            public const string Role = "Role";
            public const string Username = "Username";
            public const string Jwt = "Jwt";
            public const string IsPasswordChangeRequired = "IsPasswordChangeRequired";
        }
    }
}
