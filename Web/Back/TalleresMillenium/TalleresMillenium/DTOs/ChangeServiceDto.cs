namespace TalleresMillenium.DTOs
{
    public class ChangeServiceDto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; }
    }
}
