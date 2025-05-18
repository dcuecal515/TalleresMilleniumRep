using TalleresMillenium.DTOs;
using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class ServiceService
    {
        private readonly UnitOfWork _unitOfWork;

        public ServiceService(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<ServiceDto>> GetallService()
        {
            IEnumerable<Servicio> servicios= await _unitOfWork.ServiceRepository.GetAllService();

            var servicedto = new List<ServiceDto>();

            foreach (var servicio in servicios) {
                var dto=new ServiceDto()
                {
                    Id = servicio.Id,
                    Nombre = servicio.Nombre,
                    Imagen = servicio.Imagen,
                    Valoraciones = servicio.valoraciones.Select(v => v.Puntuacion).ToList()
                };
                servicedto.Add(dto);
            }
            return servicedto;
        }
    }
}
