using Microsoft.AspNetCore.Authorization;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Backend.WebAPI.Policies
{
    // TODO instead of anon obj.
    //public class AllowedRoles
    //{
    //    public List<int> Roles { get; set; } = new List<int>();
    //}

    public class UserIsInRoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public UserIsInRoleAuthorizationRequirement()
        {
        }
    }

    public class UserIsInRoleAuthorizationHandler : 
        AuthorizationHandler<UserIsInRoleAuthorizationRequirement, object>
    {
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            await base.HandleAsync(context);
        }

        protected override Task HandleRequirementAsync(
                AuthorizationHandlerContext context,
                UserIsInRoleAuthorizationRequirement requirement,
                object roles
            )
        {
            var role = context.User?.Claims?.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault()?.Value;
            
            if (role == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            
            if (!Enum.TryParse(role, out Roles userRole))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            Type type = roles.GetType();
            int[] allowedRoles = (int[])type?.GetProperty("roles")?.GetValue(roles, null)!;

            if (!allowedRoles.Contains((int)userRole))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
