using Microsoft.AspNetCore.Authentication;
using SimpleDataManagementSystem.Frontend.Web.Razor.Constants;
using SimpleDataManagementSystem.Frontend.Web.Razor.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.DelegatingHandlers
{
    public class RefreshAuthorizationHeaderDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _accessor;


        public RefreshAuthorizationHeaderDelegatingHandler(IServiceProvider serviceProvider, IHttpContextAccessor accessor)
        {
            _serviceProvider = serviceProvider;
            _accessor = accessor;
        }


        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = base.Send(request, cancellationToken);

            IEnumerable<string?> headerValues = null;

            bool authorizationHeader = (bool)response?.Headers?.TryGetValues("Set-Authorization", out headerValues);

            if ((authorizationHeader != null) && authorizationHeader)
            {
                string? authorizationHeaderValue = headerValues!.FirstOrDefault();
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var contextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationHeaderValue);

                    var claims = ClaimsHelper.GetClaimsFromToken(authorizationHeaderValue);

                    var identity = new ClaimsIdentity(claims, Cookies.AuthenticationCookie.Name);
                    
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    contextAccessor.HttpContext.SignInAsync(Cookies.AuthenticationCookie.Name, claimsPrincipal).GetAwaiter().GetResult();
                }
            }

            return response;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            IEnumerable<string?> headerValues = null;

            bool authorizationHeader = (bool)response?.Headers?.TryGetValues("Set-Authorization", out headerValues);
            
            if ((authorizationHeader != null) && authorizationHeader)
            {
                string? authorizationHeaderValue = headerValues!.FirstOrDefault();
                
                using (var scope = _serviceProvider.CreateScope())
                {
                    var contextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationHeaderValue);

                    var claims = ClaimsHelper.GetClaimsFromToken(authorizationHeaderValue);

                    var identity = new ClaimsIdentity(claims, Cookies.AuthenticationCookie.Name);
                    
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    await contextAccessor.HttpContext.SignInAsync(Cookies.AuthenticationCookie.Name, claimsPrincipal);
                }
            }

            return response;
        }
    }
}
