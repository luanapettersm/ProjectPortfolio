using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectPortfolio.Services
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public async Task<string> GetTokenAsync(AuthenticateModel authenticate)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtSettings:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Int32.TryParse(configuration["Authentication:JwtSettings:ExpireHours"], out int validityTime);
            var expires = DateTime.UtcNow.AddHours(validityTime);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["Authentication:JwtSettings:Issuer"],
                Audience = configuration["Authentication:JwtSettings:Audience"],
                Subject = GenerateClaims(authenticate),
                SigningCredentials = credentials,
                Expires = expires
            };

            var token = handler.CreateToken(descriptor);
            return await Task.Run(() => handler.WriteToken(token));
        }

        private static ClaimsIdentity GenerateClaims(AuthenticateModel authenticate)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, authenticate.UserName));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, ci.Claims.First().Value));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            ci.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
            return ci;
        }
    }
}