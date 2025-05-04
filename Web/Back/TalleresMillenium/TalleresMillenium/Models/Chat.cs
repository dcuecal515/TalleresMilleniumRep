namespace TalleresMillenium.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        IEnumerable<Mensaje> Mensajes { get; set; } = new List<Mensaje>();
    }
}
