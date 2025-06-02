namespace TalleresMillenium.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public string Disponible { get; set; }
        public List<int> Valoraciones { get; set; }
    }
}
