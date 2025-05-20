using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.Models;
using TalleresMillenium.DTOs;
using TalleresMillenium.Services;
using TalleresMillenium.Mappers;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly UserMapper _userMapper;
        private readonly CocheMapper _cocheMapper;
        private readonly Coche_ServicioMapper _cocheServicioMapper;

        public UserController(UserService userService, UserMapper userMapper, CocheMapper cocheMapper, Coche_ServicioMapper coche_ServicioMapper) {
            _userMapper = userMapper;
            _userService = userService;
            _cocheMapper = cocheMapper;
            _cocheServicioMapper = coche_ServicioMapper;
        }

        [HttpGet("full")]
        public async Task<UsuarioDto> GetFullUsuario([FromQuery]int id)
        {
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
    }
}
