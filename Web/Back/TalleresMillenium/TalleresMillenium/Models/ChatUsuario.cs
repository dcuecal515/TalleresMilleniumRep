namespace TalleresMillenium.Models
{
    public class ChatUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
    }
}
