using Microsoft.EntityFrameworkCore;

namespace TalleresMillenium.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario
    {
        public int Id { get; set; }
        
        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Imagen { get; set; }

        public string Rol { get; set; }

        public ICollection<Coche> Coches { get; set; } = new List<Coche>();

        public ICollection<Chat> Chats { get; set; } = new List<Chat>();
      
        public ICollection<Valoracion> valoraciones { get; set; } = new List<Valoracion>();

    }
}
