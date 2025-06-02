namespace TalleresMillenium.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Disponible { get; set; }
        public string Descripcion { get; set; }
        public string Imagen {  get; set; }

        public ICollection<Valoracion> valoraciones { get; set; } = new List<Valoracion>();
    }
}
