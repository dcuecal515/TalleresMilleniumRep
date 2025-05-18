namespace TalleresMillenium.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public IEnumerable<Usuario> Usuarios { get; set; }
    }
}
