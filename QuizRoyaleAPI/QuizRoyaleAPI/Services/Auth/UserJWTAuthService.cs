using Microsoft.IdentityModel.Tokens;
using QuizRoyaleAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace QuizRoyaleAPI.Services.Auth
{
    public class UserJWTAuthService: IAuthService
    {
        private readonly IConfiguration _configuration;

        public UserJWTAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDTO GetToken(int id)
        {
            return new TokenDTO(GenerateToken(id));
            
        }

        private string GenerateToken(int id)
        {
            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                        new Claim(ClaimTypes.Role, "Player")
                    },
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Key").Value)),
                        SecurityAlgorithms.HmacSha512Signature))
            );
        }
    }
}
