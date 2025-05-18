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
        private readonly ServiceService _service;
        public ServiceController(ServiceService serviceService) {
            _service = serviceService;
        }
        [HttpGet]
        public async Task<IEnumerable<ServiceDto>> GetAllService() {
            IEnumerable<ServiceDto> servicios=await _service.GetallService();
            return servicios;
        }

    }
}
