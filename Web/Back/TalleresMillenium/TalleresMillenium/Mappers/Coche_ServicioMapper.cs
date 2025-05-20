using TalleresMillenium.DTOs;
using TalleresMillenium.Models;

namespace TalleresMillenium.Mappers
{
    public class Coche_ServicioMapper
    {
        public Coche_ServicioDto toDto(Coche_Servicio coche_Servicio)
        {
            return new Coche_ServicioDto()
            {
                Estado = coche_Servicio.Estado,
                Descripcion = coche_Servicio.servicio.Descripcion,
                Nombre = coche_Servicio.servicio.Nombre
            };
        }
    }
}
