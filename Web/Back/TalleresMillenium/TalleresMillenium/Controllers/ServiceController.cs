using Microsoft.AspNetCore.Mvc;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using TalleresMillenium.Services;

namespace TalleresMillenium.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private readonly ServiceService _serviceService;
        private readonly UserService _userService;
        public ServiceController(ServiceService serviceService, UserService userService)
        {
            _serviceService = serviceService;
            _userService = userService;
        }
        [HttpGet]
        public async Task<ServicioFullDto> GetAllService([FromQuery] QueryDto queryDto) {
            ServicioFullDto servicios =await _serviceService.GetallService(queryDto);
            return servicios;
        }
        [HttpGet("{id}")]
        public async Task<ServicioDto> GetServiceById(int id)
        {
            ServicioDto servicioDto=await _serviceService.GetServiceById(id);
            return servicioDto;
        }
        [HttpGet("full")]
        public async Task<ICollection<ServiceAdminDto>> GetAllFullProduct()
        {
            Usuario usuario = await GetCurrentUser();
            if (usuario == null || !usuario.Rol.Equals("Admin"))
            {
                return null;
            }
            ICollection<ServiceAdminDto> serviceAdminDto = await _serviceService.GetAllServiceFull();
            return serviceAdminDto;
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
