using TalleresMillenium.DTOs;
using TalleresMillenium.Models;

namespace TalleresMillenium.Mappers
{
    public class ReviewMapper
    {
        public Valoracion Toentity(ReviewDto reviewDto) {
            return new Valoracion
            {
                Texto = reviewDto.Texto,
                Puntuacion = reviewDto.Puntuacion,
                ServicioId = reviewDto.ServicioId,
            };
        }
        public ValoracionDto Todto(Valoracion valoracion) {
            return new ValoracionDto
            {
                Puntuacion=valoracion.Puntuacion,
                Nombre=valoracion.Usuario.Name,
                Texto=valoracion.Texto,
                Imagen=valoracion.Usuario.Imagen
            };
        }
        public Valoracion ProductToentity(ReviewDto reviewDto)
        {
            return new Valoracion
            {
                Texto = reviewDto.Texto,
                Puntuacion = reviewDto.Puntuacion,
                ProductoId = reviewDto.ServicioId,
            };
        }
    }
}
