using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Services.EFServices.Identity.WebApi
{
    public class TokenService : ITokenService
    {
        private const double ExpireTimeInMinute = 30;
        public string BuildToken(string key, string issuer, UserToBuildJwtTokenViewModel user, bool rememberMe)
        {
            List<Claim> claims = [
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                ];
            user.Roles.ForEach(c => claims.Add(new Claim(ClaimTypes.Role, c)));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: rememberMe ? DateTime.Now.AddDays(90) : DateTime.Now.AddMinutes(ExpireTimeInMinute),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
