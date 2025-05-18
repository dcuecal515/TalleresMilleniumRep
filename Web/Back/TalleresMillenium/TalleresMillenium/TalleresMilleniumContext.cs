using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;

namespace TalleresMillenium
{
    public class TalleresMilleniumContext:DbContext
    {
        private readonly Settings _settings;

        public TalleresMilleniumContext(Settings settings)
        {
            _settings = settings;
        }

        private const string DATABASE_PATH = "talleresmillenium.db";

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Coche> Coches { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Mensaje> Mensajes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Valoracion> Valoraciones { get; set; }
        public DbSet<Coche_Servicio> Coche_Servicios { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
        }
    }
}
