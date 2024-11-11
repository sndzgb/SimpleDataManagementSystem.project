using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SimpleDataManagementSystem.Shared.Common.Constants;
using SimpleDataManagementSystem.Shared.Common.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        public static void AddDefaultPolicies(this AuthorizationOptions authorizationOptions)
        {
            authorizationOptions.AddPolicy(
                    Policies.PolicyNames.UserIsResourceOwner,
                    policy => policy.Requirements.Add(new UserIsResourceOwnerAuthorizationRequirement())
                );

            authorizationOptions.AddPolicy(
                Policies.PolicyNames.UserIsInRole,
                policy => policy.Requirements.Add(new UserIsInRoleAuthorizationRequirement())
            );
        }

        public static IServiceCollection AddServicesForDefaultPolicies(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, UserIsResourceOwnerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, UserIsInRoleAuthorizationHandler>();

            return services;
        }
    }
}
