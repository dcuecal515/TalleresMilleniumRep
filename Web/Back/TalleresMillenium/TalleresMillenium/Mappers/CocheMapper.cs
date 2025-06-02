using TalleresMillenium.DTOs;
using TalleresMillenium.Models;

namespace TalleresMillenium.Mappers
{
    public class CocheMapper
    {
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
