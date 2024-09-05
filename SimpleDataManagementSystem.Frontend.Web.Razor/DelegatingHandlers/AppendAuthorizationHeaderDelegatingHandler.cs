using Microsoft.AspNetCore.Authentication;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.Net.Http.Headers;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.DelegatingHandlers
{
    public class AppendAuthorizationHeaderDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;


        public AppendAuthorizationHeaderDelegatingHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                string? token = null;

                var contextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

                token = contextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ExtendedClaims.Type.Jwt)?.FirstOrDefault()?.Value;

                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return base.Send(request, cancellationToken);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                string? token = null;

                var contextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

                token = contextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == ExtendedClaims.Type.Jwt)?.FirstOrDefault()?.Value;

                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
