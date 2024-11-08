using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Horizons.Core.Auth.Identity.Interface;
using Horizons.Core.Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Horizons.Core.Auth.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
            };

            if (user.PhoneNumber is not null)
            {
                claims.Add(new(ClaimTypes.Name, user.PhoneNumber));
            }

            foreach(var userRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(_jwtOptions.JwtExpireDays);

            var token = new JwtSecurityToken(
                _jwtOptions.JwtAudience,
                _jwtOptions.JwtAudience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
