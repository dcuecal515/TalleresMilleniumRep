namespace TalleresMillenium.DTOs
{
    public class ElementoCarritoDto
    {
        public string Tipo { get; set; }
        public string Matricula { get; set; }
        public List<ServicioCarritoDto> Servicios { get; set; }
    }
}
