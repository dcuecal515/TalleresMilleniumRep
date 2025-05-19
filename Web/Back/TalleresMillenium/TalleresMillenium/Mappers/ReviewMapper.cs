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
    }
}
