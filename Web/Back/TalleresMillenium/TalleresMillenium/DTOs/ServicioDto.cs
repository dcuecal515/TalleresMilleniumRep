using TalleresMillenium.Models;

namespace TalleresMillenium.DTOs
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public ICollection<ValoracionDto> valoracionesDto { get; set; } = new List<ValoracionDto>();

    }
}
