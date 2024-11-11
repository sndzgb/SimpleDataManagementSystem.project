﻿using SimpleDataManagementSystem.Shared.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Helpers
{
    public static class ClaimsHelper
    {
        // TODO add error handling
        // TODO move to shared project
        // TODO put claims in Token
        public static List<Claim>? GetClaimsFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, jwt.Claims.Where(x => x.Type == ExtendedClaims.Type.Username).Single().Value));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, jwt.Claims.Where(x => x.Type == ExtendedClaims.Type.UserId).Single().Value));
            claims.Add(new Claim(ClaimTypes.Role, jwt.Claims.Where(x => x.Type == ClaimTypes.Role).Single().Value));
            claims.Add(new Claim(ExtendedClaims.Type.Jwt, token));
            claims.Add(new Claim(ExtendedClaims.Type.IsPasswordChangeRequired, jwt.Claims.Where(x => x.Type == ExtendedClaims.Type.IsPasswordChangeRequired).Single().Value));
            claims.Add(new Claim(ExtendedClaims.Type.Username, jwt.Claims.Where(x => x.Type == ExtendedClaims.Type.Username).Single().Value));

            return claims;
        }

        public static string? GetValueFromClaim(Claim claim)
        {
            return GetClaimsFromToken(claim.Type)?.FirstOrDefault()?.Value;
        }

        public static string? GetValueFromClaimsByKey(IEnumerable<Claim> claims, string key)
        {
            var cs = claims.Where(x => x.Type == key);
            var val = cs.FirstOrDefault()?.Value;
            return val;
        }
    }
}
