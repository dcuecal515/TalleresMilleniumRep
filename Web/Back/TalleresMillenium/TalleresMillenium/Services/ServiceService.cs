using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TalleresMillenium.Services
{
    public class ServiceService
    {
        private readonly UnitOfWork _unitOfWork;

        public ServiceService(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServicioFullDto> GetallService( QueryDto queryDto)
        {
            IEnumerable<Servicio> servicios= await _unitOfWork.ServiceRepository.GetAllService();
            var servicedto = new List<ServiceDto>();
            var totalservicios = servicios.Count();

            foreach (var servicio in servicios)
            {
                var dto = new ServiceDto()
                {
                    Id = servicio.Id,
                    Nombre = servicio.Nombre,
                    Imagen = servicio.Imagen,
                    Valoraciones = servicio.valoraciones.Select(v => v.Puntuacion).ToList()
                };
                servicedto.Add(dto);
            }
            ServicioFullDto servicioFullDto = new ServicioFullDto { serviceDtos = servicedto, totalservice = totalservicios };

            servicioFullDto.serviceDtos = servicioFullDto.serviceDtos.Skip((queryDto.ActualPage - 1) * queryDto.ServicePageSize).Take(queryDto.ServicePageSize).ToList();
            return servicioFullDto;
        }
        public async Task<Servicio> GetServiceById(int id)
        {
            Servicio servicio=await _unitOfWork.ServiceRepository.GetServiceById(id);
            return servicio;
        }
    }
}
