using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Infrastructure.Services.TokenServices
{
    public class GenerateTokenJwt : ITokenService
    {
        private readonly IConfiguration _configuration;

        public GenerateTokenJwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string CreateToken(User userModel)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!);

            var clains = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, userModel.Email!),
                new Claim(JwtRegisteredClaimNames.Name, userModel.UserName!)
            };

            var signInCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var configToken = new SecurityTokenDescriptor
            {
                SigningCredentials = signInCredentials,
                Expires = DateTime.UtcNow.AddDays(3),
                Subject = new ClaimsIdentity(clains),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"]
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(configToken);

            return handler.WriteToken(token);
        }
    }
}
