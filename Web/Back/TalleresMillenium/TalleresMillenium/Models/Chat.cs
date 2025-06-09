namespace TalleresMillenium.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public ICollection<ChatUsuario> ChatUsuarios { get; set; } = new List<ChatUsuario>();
        public ICollection<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }
}
