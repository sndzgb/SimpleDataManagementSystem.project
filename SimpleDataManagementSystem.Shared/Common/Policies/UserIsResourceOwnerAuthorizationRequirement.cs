using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Common.Policies
{
    public class UserIsResourceOwnerAuthorizationRequirement : IAuthorizationRequirement
    {
        public UserIsResourceOwnerAuthorizationRequirement()
        {
        }
    }

    public class UserIsResourceOwnerAuthorizationHandler :
        AuthorizationHandler<UserIsResourceOwnerAuthorizationRequirement, object>
    {
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            await base.HandleAsync(context);
        }

        protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                UserIsResourceOwnerAuthorizationRequirement requirement,
                object resourceId
            )
        {
            var userId = context.User?.Identity?.Name;

            if (resourceId.ToString() != userId)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
