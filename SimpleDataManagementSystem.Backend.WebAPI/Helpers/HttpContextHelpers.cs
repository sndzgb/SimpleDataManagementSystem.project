using Microsoft.AspNetCore.Http;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Backend.WebAPI.Helpers
{
    public static class HttpContextHelpers
    {
        public static int? GetAuthenticatedUserIdFromHttpContext(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return null;
            }

            if (
                int.TryParse
                (
                    httpContext
                    !.User
                    !.Claims
                    ?.Where(x => x.Type == ExtendedClaims.Type.UserId)
                    .FirstOrDefault()
                    ?.Value, out int userId
                )
            )
            {
                return userId;
            }
            else
            {
                return null;
            }
        }

        public static string GetClaimValueByType(string claimType, HttpContext httpContext)
        {
            return httpContext
                !.User
                !.Claims
                ?.Where(x => x.Type == claimType)
                .FirstOrDefault()
                ?.Value ?? "";
        }
    }
}
