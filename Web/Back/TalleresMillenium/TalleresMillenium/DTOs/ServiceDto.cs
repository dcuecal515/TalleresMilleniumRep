namespace TalleresMillenium.DTOs
{
    public class ServiceDto
    {
        public int Id {  get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public List<int> Valoraciones { get; set; }
    }
}
