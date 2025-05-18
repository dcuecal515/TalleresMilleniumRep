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
        public async Task<IEnumerable<ServiceDto>> GetAllService() {
            IEnumerable<ServiceDto> servicios=await _serviceService.GetallService();
            return servicios;
        }
        [HttpGet("{id}")]
        public async Task<Servicio> GetServiceById(int id)
        {
            Servicio servicio=await _serviceService.GetServiceById(id);
            return servicio;
        }

    }
}
