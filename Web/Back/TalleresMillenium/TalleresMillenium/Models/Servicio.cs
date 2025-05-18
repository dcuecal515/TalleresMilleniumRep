namespace TalleresMillenium.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public ICollection<Coche_Servicio> coche_Servicios { get; set; } = new List<Coche_Servicio>();
        public ICollection<Valoracion> valoraciones { get; set; } = new List<Valoracion>();
    }
}
