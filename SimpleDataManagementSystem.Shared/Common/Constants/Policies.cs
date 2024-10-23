using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Common.Constants
{
    public class Policies
    {
        public PolicyNames Names; // { get; set; }
        
        public class PolicyNames
        {
            public const string UserIsResourceOwner = "UserIsResourceOwnerPolicy";
            public const string UserIsInRole = "UserIsInRolePolicy";
        }
    }
}
