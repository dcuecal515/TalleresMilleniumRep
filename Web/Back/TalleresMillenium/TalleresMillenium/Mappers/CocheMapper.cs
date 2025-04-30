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
    }
}
