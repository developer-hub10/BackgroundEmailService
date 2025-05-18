using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace BackgroundEmailService.Services
{


    public class JwtService
    {

        private readonly IConfiguration _config;
         
        
        public JwtService(IConfiguration config)
        {
           _config = config;
        }


        public  string GenerateToken(int id, string username, string role)
        {
            
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
               _config.GetValue<string>("AppSettings:Token")!
            ));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _config.GetValue<string>("AppSettings:Issuer"),
                audience: _config.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: cred
            );
            
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

    }
}