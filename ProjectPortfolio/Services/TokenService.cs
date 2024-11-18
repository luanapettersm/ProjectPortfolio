using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectPortfolio.Services
{
    public class TokenService : ITokenService
    {
        public string GetToken(SystemUserModel systemUser)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(systemUser),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(10)
            };

            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(SystemUserModel systemUser)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(
                new Claim(ClaimTypes.Name, systemUser.UserName));
            foreach(var role in systemUser.SystemRole.ToString())
                ci.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
            return ci;
        }
    }
}