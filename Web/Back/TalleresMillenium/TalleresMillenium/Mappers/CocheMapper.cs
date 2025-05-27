using TalleresMillenium.DTOs;
using TalleresMillenium.Models;

namespace TalleresMillenium.Mappers
{
    public class CocheMapper
    {
        public Coche toEntity(SignUpDto userDto)
        {
            return new Coche
            {
                Matricula = userDto.matricula,
                Tipo = userDto.tipo_vehiculo,
                Combustible = userDto.tipo_combustible,
                Fecha_itv = userDto.fecha_ITV
            };
        }
        public CocheDto toDto(Coche coche)
        {
            return new CocheDto
            {
                Tipo = coche.Tipo,
                Combustible = coche.Combustible,
                Fecha_itv = coche.Fecha_itv,
                Imagen = coche.Imagen,
                Kilometraje = coche.Kilometraje,
                Matricula = coche.Matricula
            };
        }
    }
}
