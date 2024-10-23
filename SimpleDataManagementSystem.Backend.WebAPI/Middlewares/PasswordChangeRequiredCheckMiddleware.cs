using Microsoft.AspNetCore.Authorization;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net;

namespace SimpleDataManagementSystem.Backend.WebAPI.Middlewares
{
    public class PasswordChangeRequiredCheckMiddleware
    {
        private readonly ILogger<PasswordChangeRequiredCheckMiddleware> _logger;
        private readonly RequestDelegate _next;


        public PasswordChangeRequiredCheckMiddleware(ILogger<PasswordChangeRequiredCheckMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isPasswordChangeRequired = Convert.ToBoolean(
                context
                    ?.User
                    ?.Claims
                    ?.Where(x => x.Type == ExtendedClaims.Type.IsPasswordChangeRequired)
                    ?.FirstOrDefault()
                    ?.Value
            );

            var endpoint = context!.GetEndpoint();

            if (
                    (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null) // authenticated user required
                    &&
                    (isPasswordChangeRequired) // password changed required
                )
            {
                context!.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context!);
        }
    }
}
