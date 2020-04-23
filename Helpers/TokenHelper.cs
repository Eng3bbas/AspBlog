using BlogAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BlogAPI.Helpers
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration configuration;
        public TokenHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            SecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
               Issuer = configuration["JWT:Issuer"],
               Audience = configuration["JWT:Issuer"],
               Subject = new ClaimsIdentity(new []
               {
                   new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                   new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                   new Claim(JwtRegisteredClaimNames.Email,user.Email),
                   new Claim(JwtRegisteredClaimNames.AuthTime,DateTime.UtcNow.ToString()),
                   new Claim(JwtRegisteredClaimNames.GivenName , user.Name),
                   new Claim("userId" , user.Id.ToString()),
                   new Claim("role",user.IsAdmin ? "admin":"user")
               }),
               Expires = DateTime.UtcNow.AddYears(1),
               SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),SecurityAlgorithms.HmacSha256Signature)
            };
            var token =  handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }
    }
}
