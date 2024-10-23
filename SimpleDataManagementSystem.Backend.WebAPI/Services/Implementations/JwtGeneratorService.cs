using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleDataManagementSystem.Backend.WebAPI.Options;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services.Implementations
{
    public class JwtGeneratorService : ITokenGeneratorService
    {
        private readonly IOptions<JwtOptions> _jwtOptions;


        public JwtGeneratorService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions;

            if (jwtOptions == null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }
        }


        public Task<string?> GenerateTokenAsync(AuthenticatedUser authenticatedUser)
        {
            if (authenticatedUser == null)
            {
                return Task.FromResult<string?>(null);
            }

            var handler = new JwtSecurityTokenHandler();
            
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Value.Key);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(authenticatedUser),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.ExpiresInMinutes),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = credentials,
                Audience = _jwtOptions.Value.Audience,
                Issuer = _jwtOptions.Value.Issuer
            };

            var token = handler.CreateToken(tokenDescriptor);

            return Task.FromResult<string?>(handler.WriteToken(token));
        }

        private ClaimsIdentity GenerateClaims(AuthenticatedUser user)
        {
            var claims = new ClaimsIdentity();

            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserId.ToString()));
            claims.AddClaim(new Claim(ExtendedClaims.Type.UserId, user.UserId.ToString()));
            claims.AddClaim(new Claim(ExtendedClaims.Type.Username, user.Username));
            claims.AddClaim(new Claim(ExtendedClaims.Type.IsPasswordChangeRequired, user.IsPasswordChangeRequired.ToString()));
            
            foreach (var role in user.Roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
