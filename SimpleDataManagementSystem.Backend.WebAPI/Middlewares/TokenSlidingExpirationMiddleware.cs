using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Backend.WebAPI.Middlewares
{
    public class TokenSlidingExpirationMiddleware
    {
        private readonly ILogger<TokenSlidingExpirationMiddleware> _logger;
        private readonly RequestDelegate _next;


        public TokenSlidingExpirationMiddleware(ILogger<TokenSlidingExpirationMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, ITokenGeneratorService tokenGeneratorService)
        {
            try
            {
                JwtSecurityToken? token = null;

                var headers = context.Request.Headers;

                if (context.Request.Headers.ContainsKey("Authorization"))
                {
                    if (context.Request.Headers["Authorization"][0]!.StartsWith("Bearer "))
                    {
                        string authorizationHeader = context.Request.Headers["Authorization"][0]!;

                        token = new JwtSecurityTokenHandler().ReadJwtToken(authorizationHeader[7..]); // remove "Bearer "

                        if (token != null)
                        {
                            // check if token is not expired
                            if (token.ValidTo > DateTime.UtcNow)
                            {
                                TimeSpan timeElapsed = DateTime.UtcNow.Subtract(token.ValidFrom);
                                TimeSpan timeRemaining = token.ValidTo.Subtract(DateTime.UtcNow);

                                // if more than half of the timeout interval has elapsed
                                if (timeRemaining < timeElapsed)
                                {
                                    var jwtToken = await tokenGeneratorService.GenerateTokenAsync(
                                        new AuthenticatedUser(
                                            Convert.ToInt32(token.Claims.Where(x => x.Type == ExtendedClaims.Type.UserId).FirstOrDefault().Value),
                                            token.Claims.Where(x => x.Type == ExtendedClaims.Type.Username).First().Value,
                                            token.Claims.Where(x => x.Type == ClaimTypes.Role).ToList().Select(x => x.Value).ToArray()
                                        )
                                    );

                                    context.Response.OnStarting(() =>
                                    {
                                        context.Response.Headers.Append("Set-Authorization", jwtToken);
                                        return Task.CompletedTask;
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(exception: e, string.Empty);
            }

            await _next(context);
        }
    }
}
