namespace TalleresMillenium.Models
{
    public class Mensaje
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
        public Chat Chat { get; set; }
        public int ChatId { get; set; }

        public string Texto { get; set; }
    }
}
