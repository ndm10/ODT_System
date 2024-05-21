using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ODT_System.Models;
using ODT_System.SharedObject;
using ODT_System.Utils.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ODT_System.Utils
{
    public class JWTHandler : IJWTHandler
    {
        private readonly AppSettings _appSettings;

        public JWTHandler(IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _appSettings = optionsMonitor.CurrentValue;
        }

        public object GenerateToken(User user)
        {
            var secretKey = _appSettings.Secret;

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            // Create the claims
            var subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            });

            // Set the expire time
            var expire = System.DateTime.UtcNow.AddMinutes(180);

            // Create the signing credentials
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expire,
                SigningCredentials = signingCredentials
            };

            //Create the token
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
