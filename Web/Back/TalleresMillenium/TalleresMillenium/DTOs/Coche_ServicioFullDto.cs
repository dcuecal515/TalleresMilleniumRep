namespace TalleresMillenium.DTOs
{
    public class Coche_ServicioFullDto
    {
        public string Estado { get; set; }
        public DateOnly Fecha { get; set; }

        public List<ServicioCocheDto> Servicios { get; set; }

        public string Matricula { get; set; }
        public string Tipo { get; set; }
    }
}
