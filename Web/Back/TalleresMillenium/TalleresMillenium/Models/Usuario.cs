using Microsoft.EntityFrameworkCore;

namespace TalleresMillenium.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usuario
    {
        public int Id { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        public string imagen { get; set; }

        public string rol { get; set; }

        public ICollection<Coche> Coches { get; set; } = new List<Coche>();

    }
}
