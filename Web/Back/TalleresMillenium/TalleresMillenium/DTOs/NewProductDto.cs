namespace TalleresMillenium.DTOs
{
    public class NewProductDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; }
        public string Disponible { get; set; }
    }
}
