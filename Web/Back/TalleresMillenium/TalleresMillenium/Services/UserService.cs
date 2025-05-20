using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using TalleresMillenium.Mappers;

namespace TalleresMillenium.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenParameters;
        private readonly UserMapper _userMapper;
        private readonly CocheMapper _cocheMapper;
        private readonly ImageService _imageService;

        public UserService(UnitOfWork unitOfWork, IOptionsMonitor<JwtBearerOptions> jwtOptions, UserMapper userMapper, CocheMapper cocheMapper, ImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme)
                .TokenValidationParameters;
            _userMapper = userMapper;
            _cocheMapper = cocheMapper;
            _imageService = imageService;
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
        public async Task<Boolean> GetIfEmailExists (string email)
        {
            Usuario user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            } else
            {
                return true;
            }
        }

        public async Task<string> RegisterUser(SignUpDto receivedUser)
        {
            Usuario user = _userMapper.toEntity(receivedUser);
            Coche coche = _cocheMapper.toEntity(receivedUser);
            Coche cocheExistente = await _unitOfWork.CocheRepository.GetByMatriculaAsync(receivedUser.matricula);
            if (cocheExistente != null)
            {
                return null;
            }
            PasswordService passwordService = new PasswordService();
            user.Password = passwordService.Hash(receivedUser.contrasena);
            if (receivedUser.imagenPerfil != null)
            {
                user.Imagen = "/" + await _imageService.InsertAsync(receivedUser.imagenPerfil);
            } else
            {
                user.Imagen = "/images/perfilDefect.webp";
            }
            user.Rol = "User";
            coche.Imagen = "/" + await _imageService.InsertAsync(receivedUser.imagenFT);
            coche.Kilometraje = 0;
            user.Coches.Add(coche);
            await _unitOfWork.CocheRepository.InsertAsync(coche);

            Usuario newUser = await InsertUserAsync(user);

            return ObtainToken(newUser);
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
        public async Task<Usuario> InsertUserAsync(Usuario user)
        {
            // Hace falta añadir aqui un nuevo historial
            Usuario newUser = await _unitOfWork.UserRepository.InsertAsync(user);
            await _unitOfWork.SaveAsync();
            return newUser;
        }

        public async Task<Usuario> GetUserFromDbByStringId(string stringId)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(Int32.Parse(stringId));
        }

        public async Task<Usuario> GetFullUserById(int id)
        {
            return await _unitOfWork.UserRepository.GetFullUserById(id);
        }
    }
}
