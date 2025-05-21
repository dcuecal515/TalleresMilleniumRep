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
        public ServiceController(ServiceService serviceService) {
            _serviceService = serviceService;
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

    }
}
