using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace SimpleDataManagementSystem.Backend.WebAPI.Middlewares
{
    public class AllowOnlyAuthenticatedMiddleware
    {
        private readonly ILogger<AllowOnlyAuthenticatedMiddleware> _logger;
        private readonly RequestDelegate _next;


        public AllowOnlyAuthenticatedMiddleware(ILogger<AllowOnlyAuthenticatedMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isAuthenticated = context?.User?.Identity?.IsAuthenticated ?? false;

            var endpoint = context!.GetEndpoint();

            if (
                    (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null)
                    &&
                    (!isAuthenticated)
                )
            {
                context!.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context!);
        }
    }
}
