namespace TalleresMillenium.Models
{
    public class Valoracion
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int Puntuacion { get; set; }
        public Usuario Usuario { get; set; }

        public int UsuarioId { get; set; }
        public Servicio Servicio { get; set; }

        public int? ServicioId { get; set; }
        public Producto Producto { get; set; }

        public int? ProductoId { get; set; }
    }
}
