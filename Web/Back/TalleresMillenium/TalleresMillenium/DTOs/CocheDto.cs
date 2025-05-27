namespace TalleresMillenium.DTOs
{
    public class CocheDto
    {
        public string Tipo { get; set; }
        public string Imagen { get; set; }
        public string Matricula { get; set; }
        public string Fecha_itv { get; set; }
        public string Combustible { get; set; }
        public int ?Kilometraje { get; set; }
        public List<Coche_ServicioDto> Servicios { get; set; }
    }
}
