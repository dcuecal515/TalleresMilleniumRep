using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenParameters;

        public UserService(UnitOfWork unitOfWork, IOptionsMonitor<JwtBearerOptions> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme)
                .TokenValidationParameters;
        }
        public async Task<Usuario> GetUserByEmailAndPassword(string email, string password)
        {
            Usuario user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            if (user == null) {
                return null;
            }
            PasswordService passwordService = new PasswordService();
            if (passwordService.IsPasswordCorrect(user.Password, password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public string ObtainToken(Usuario user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                    {
                        { "id", user.Id },
                        { "email", user.Email },
                        { "name", user.Name },
                        { ClaimTypes.Role, user.Rol }
                    },
                Expires = DateTime.UtcNow.AddYears(3),
                SigningCredentials = new SigningCredentials(
                        _tokenParameters.IssuerSigningKey,
                        SecurityAlgorithms.HmacSha256Signature
                    )
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
