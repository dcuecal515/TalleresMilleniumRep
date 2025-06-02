namespace TalleresMillenium.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string Disponible { get; set; }
        public ICollection<ValoracionDto> valoracionesDto { get; set; } = new List<ValoracionDto>();
    }
}
