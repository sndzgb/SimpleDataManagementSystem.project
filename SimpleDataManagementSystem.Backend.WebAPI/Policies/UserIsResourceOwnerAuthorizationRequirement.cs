using Microsoft.AspNetCore.Authorization;
using SimpleDataManagementSystem.Backend.Logic.Models;
using SimpleDataManagementSystem.Backend.WebAPI.Helpers;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Diagnostics;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Backend.WebAPI.Policies
{
    public class UserIsResourceOwnerAuthorizationRequirement : IAuthorizationRequirement
    {
        //public int UserId { get; private set; }


        public UserIsResourceOwnerAuthorizationRequirement(/*int userId*/)
        {
            //UserId = userId;
        }
    }

    public class UserIsResourceOwnerAuthorizationHandler : //IAuthorizationHandler
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
            // OLD WAY
            //int userId = Convert.ToInt32(context.User?.Identity?.Name);

            //var claim = (context.User?.Identity as ClaimsIdentity)
            //    ?.Claims
            //    .Where(c => c.Type == "UserId")
            //    .FirstOrDefault();

            //if (userId == null) 
            //{
            //    context.Fail();
            //    return Task.CompletedTask;
            //}

            //if (userId != resourceId)
            //{
            //    context.Fail();
            //    return Task.CompletedTask;
            //}

            // NEW WAY
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
