using System.Globalization;
using System.Text;
using System.Xml.Linq;
using TalleresMillenium.DTOs;
using TalleresMillenium.Mappers;
using TalleresMillenium.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TalleresMillenium.Services
{
    public class ServiceService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ReviewMapper _reviewMapper;

        public ServiceService(UnitOfWork unitOfWork,ReviewMapper reviewMapper)
        {
            _unitOfWork = unitOfWork;
            _reviewMapper = reviewMapper;
        }
        public async Task<ServicioFullDto> GetallService( QueryDto queryDto)
        {
            IEnumerable<Servicio> servicios= await _unitOfWork.ServiceRepository.GetAllService();
            var servicedto = new List<ServiceDto>();
            var totalservicios=0;
            if (queryDto.busqueda != null) {
                string separatebusqueda = queryDto.busqueda.Normalize(NormalizationForm.FormD);

                StringBuilder newname = new StringBuilder();
                foreach (char c in separatebusqueda)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        newname.Append(c);
                    }
                }
                string searchbusqueda = newname.ToString().Normalize(NormalizationForm.FormC);

                foreach (Servicio servicio in servicios)
                {
                    string separatenamedatabase = servicio.Nombre.Normalize(NormalizationForm.FormD);

                    StringBuilder newnamedatabase = new StringBuilder();
                    foreach (char c in separatenamedatabase)
                    {
                        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        {
                            newnamedatabase.Append(c);
                        }
                    }
                    string searchnamedatabase = newnamedatabase.ToString().Normalize(NormalizationForm.FormC);

                    if (searchnamedatabase.ToLower().Contains(searchbusqueda.ToLower()))
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
                }
                totalservicios = servicedto.Count();
            }
            else
            {
                totalservicios = servicios.Count();
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
            }
            ServicioFullDto servicioFullDto = new ServicioFullDto { serviceDtos = servicedto, totalservice = totalservicios };

            servicioFullDto.serviceDtos = servicioFullDto.serviceDtos.Skip((queryDto.ActualPage - 1) * queryDto.ServicePageSize).Take(queryDto.ServicePageSize).ToList();
            return servicioFullDto;
        }
        public async Task<ServicioDto> GetServiceById(int id)
        {
            Servicio servicio=await _unitOfWork.ServiceRepository.GetServiceById(id);
            ServicioDto servicioDto = new ServicioDto();

            servicioDto.Id = id;
            servicioDto.Nombre = servicio.Nombre;
            servicioDto.Descripcion = servicio.Descripcion;
            servicioDto.Imagen=servicio.Imagen;
            foreach(Valoracion valoracion in servicio.valoraciones)
            {
                ValoracionDto valoracionDto = _reviewMapper.Todto(valoracion);
                servicioDto.valoracionesDto.Add(valoracionDto);
            }

            return servicioDto;
        }
        public async Task<ICollection<ServiceAdminDto>> GetAllServiceFull()
        {
            IEnumerable<Servicio> servicios = await _unitOfWork.ServiceRepository.GetAllServiceFull();
            var services = new List<ServiceAdminDto>();
            foreach (Servicio servicio in servicios)
            {
                ServiceAdminDto serviceAdminDto = new ServiceAdminDto();
                serviceAdminDto.Id = servicio.Id;
                serviceAdminDto.Nombre = servicio.Nombre;
                serviceAdminDto.Descripcion = servicio.Descripcion;
                serviceAdminDto.Imagen = servicio.Imagen;
                services.Add(serviceAdminDto);
            }
            return services;
        }
        public async Task<Servicio> getServiceByIdOnlyAsync(int id)
        {
            return await _unitOfWork.ServiceRepository.GetByIdAsync(id);
        }
        public async Task UpdateService(Servicio servicio)
        {
            _unitOfWork.ServiceRepository.Update(servicio);
            await _unitOfWork.SaveAsync();
        }
        public async Task InsertService(Servicio servicio)
        {
            await _unitOfWork.ServiceRepository.InsertAsync(servicio);
            await _unitOfWork.SaveAsync();
        }
    }
}
