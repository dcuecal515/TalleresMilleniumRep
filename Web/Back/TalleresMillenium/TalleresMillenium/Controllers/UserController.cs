using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.Models;
using TalleresMillenium.DTOs;
using TalleresMillenium.Services;
using TalleresMillenium.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly UserMapper _userMapper;
        private readonly CocheService _cocheService;
        private readonly CocheMapper _cocheMapper;
        private readonly Coche_ServicioMapper _cocheServicioMapper;
        private readonly Coche_ServicioService _coche_ServicioService;
        private readonly ChatUsuarioService _chatUsuarioService;
        private readonly ChatService _chatService;

        public UserController(UserService userService, UserMapper userMapper, CocheMapper cocheMapper, Coche_ServicioMapper coche_ServicioMapper, CocheService cocheService, Coche_ServicioService coche_ServicioService,ChatUsuarioService chatUsuarioService, ChatService chatService)
        {
            _userMapper = userMapper;
            _userService = userService;
            _cocheMapper = cocheMapper;
            _cocheServicioMapper = coche_ServicioMapper;
            _cocheService = cocheService;
            _coche_ServicioService = coche_ServicioService;
            _chatUsuarioService = chatUsuarioService;
            _chatService = chatService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ICollection<ListuserDto>> GetAllUsers()
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return null;
            }
            IEnumerable<Usuario> usuarios = await _userService.GetAllUsers(usuario.Id);
            ICollection<ListuserDto> users = _userMapper.ListtoDto(usuarios.ToList());
            return users;
        }

        [Authorize]
        [HttpGet("full")]
        public async Task<UsuarioDto> GetFullUsuario([FromQuery] int id)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return null;
            }
            Usuario user = await _userService.GetFullUserById(id);
            UsuarioDto usuarioDto = _userMapper.toDto(user);
            List<CocheDto> cocheDtos = new List<CocheDto>();


            foreach (var coche in user.Coches)
            {
                List<Coche_ServicioDto> coche_servicioDtos = new List<Coche_ServicioDto>();
                CocheDto cocheDto = _cocheMapper.toDto(coche);
                foreach (var coche_Servicio in coche.coche_Servicios)
                {
                    Coche_ServicioDto coche_ServicioDto = _cocheServicioMapper.toDto(coche_Servicio);
                    coche_servicioDtos.Add(coche_ServicioDto);
                }
                cocheDto.Servicios = coche_servicioDtos;
                cocheDtos.Add(cocheDto);
            }
            usuarioDto.Coches = cocheDtos;

            return usuarioDto;
        }

        [Authorize]
        [HttpPost("nombre")]
        public async Task<string> changeName([FromBody] NombreDto nombreDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return null;
            }
            usuario.Name = nombreDto.Nombre;

            Usuario userUpdated = await _userService.updateUser(usuario);
            return userUpdated.Name;
        }

        [Authorize]
        [HttpPost("email")]
        public async Task<IActionResult> changeEmail([FromBody] EmailDto emailDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return Unauthorized();
            }
            Boolean returnResult = await _userService.GetIfEmailExists(emailDto.Email);
            if (returnResult)
            {
                return Unauthorized();
            } else
            {

                usuario.Email = emailDto.Email;
                Usuario userUpdated = await _userService.updateUser(usuario);
                return Ok();
            }
        }

        [Authorize]
        [HttpPost("contrasena")]
        public async Task<IActionResult> changePassword([FromBody] ContrasenaDto contrasenaDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return Unauthorized();
            }
            PasswordService passwordService = new PasswordService();

            bool iscorrect = passwordService.IsPasswordCorrect(usuario.Password, contrasenaDto.OldContrasena);
            if (iscorrect) {
                usuario.Password = passwordService.Hash(contrasenaDto.NewContrasena);
                Usuario userUpdated = await _userService.updateUser(usuario);
                return Ok();
            } else
            {
                return Unauthorized();
            }

        }

        [Authorize]
        [HttpPut("image")]
        public async Task<ImageSendDto> changeimage([FromForm] ImageDto imageDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return null;
            }
            if (imageDto.Image != null)
            {
                ImageService imageService = new ImageService();
                usuario.Imagen = "/" + await imageService.InsertAsync(imageDto.Image);
            } else
            {
                usuario.Imagen = "/images/perfilDefect.webp";
            }
            await _userService.updateUser(usuario);
            return new ImageSendDto { Image = usuario.Imagen };
        }

        [Authorize]
        [HttpPost("coche")]
        public async Task<CocheDto> newCar([FromForm] NewCocheDto newCocheDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null)
            {
                return null;
            }

            bool matriculaExists = await _userService.GetIfMatriculaExists(newCocheDto.Matricula);

            Usuario fullUser = await _userService.GetFullUserById(usuario.Id);
            if (matriculaExists)
            {
                return null;
            } else
            {
                ImageService imageService = new ImageService();
                Coche coche = new Coche
                {
                    UsuarioId = fullUser.Id,
                    coche_Servicios = new List<Coche_Servicio>(),
                    Combustible = newCocheDto.Combustible,
                    Fecha_itv = newCocheDto.Fecha_itv,
                    Imagen = "/" + await imageService.InsertAsync(newCocheDto.Imagen),
                    Kilometraje = int.Parse(newCocheDto.Kilometraje),
                    Matricula = newCocheDto.Matricula,
                    Tipo = newCocheDto.Tipo
                };
                fullUser.Coches.Add(coche);
                Coche updCoche = await _cocheService.InsertCocheAsync(coche);
                CocheDto cocheDto = _cocheMapper.toDto(updCoche);
                return cocheDto;
            }
        }
        [Authorize]
        [HttpPut("change")]
        public async Task<IActionResult> PutChangeRol([FromBody] ChangeRolDto changeRolDto)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return Unauthorized();
            }
            Usuario userchange = await _userService.getUserByIdOnlyAsync(changeRolDto.Id);
            if (userchange != null)
            {
                userchange.Rol = changeRolDto.Rol;
            }
            else
            {
                return null;
            }
            await _userService.UpdateUser(userchange);
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public async Task DeleteUser([FromQuery] int id)
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin") || usuario.Id == id)
            {
                return;
            }
            ICollection<ChatUsuario> chatUsuarios = await _chatUsuarioService.GetAllChatUser(id);
            List<int> ids = new List<int>();
            foreach (var chatUser in chatUsuarios)
            {
                ids.Add(chatUser.ChatId);
            }
            await _chatService.DeleteManyChats(ids);
            Usuario deletedUser = await _userService.getUserByIdOnlyAsync(id);

            if (deletedUser == null)
            {
                return;
            }

            await _userService.DeleteUser(deletedUser);
        }

        private async Task<Usuario> GetCurrentUser()
        {
            // Pilla el usuario autenticado según ASP
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string idString = currentUser.Claims.First().ToString().Substring(3); // 3 porque en las propiedades sale "id: X", y la X sale en la tercera posición

            // Pilla el usuario de la base de datos
            return await _userService.GetUserFromDbByStringId(idString);
        }
    }
}
