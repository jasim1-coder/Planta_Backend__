using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities;

namespace Common.Helpers
{
    public class JWTGenerator
    {
        private readonly IConfiguration config;
        public JWTGenerator(IConfiguration _config)
        {
            config = _config;
        }

        //public async Task<string> GenerateToken(User userdata)
        //{

        //    var keyString = config["Jwt:Key"];
        //    if (string.IsNullOrEmpty(keyString))
        //        throw new Exception("JWT Key is missing from config!");

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userdata.UserID.ToString()),
        //        new Claim(ClaimTypes.Name, userdata.Name),
        //        new Claim(ClaimTypes.Role, userdata.Role)
        //    };

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(60),
        //        signingCredentials: creds
        //    );
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        public string GenerateToken(Guid userId, string role = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };
            if (!string.IsNullOrEmpty(role))
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryMinutes = int.Parse(config["Jwt:AccessTokenExpiryMinutes"] ?? "60");
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
