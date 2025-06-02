using System.Globalization;

namespace TalleresMillenium.DTOs
{
    public class ChangeProductDto
    {
        public string Id {  get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IFormFile Imagen { get; set; }
        public string Disponible { get; set; }
    }
}
