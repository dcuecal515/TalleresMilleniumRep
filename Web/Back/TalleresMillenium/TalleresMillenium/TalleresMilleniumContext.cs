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
        public DbSet<ChatUsuario> ChatUsuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #if DEBUG
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
            #else
                string connection = "Server=db20891.databaseasp.net; Database=db20891; Uid=db20891; Pwd=bF+9Z2t!x%5J;";
                optionsBuilder.UseMySql(connection,ServerVersion.AutoDetect(connection));
            #endif
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Valoracion>()
                .HasOne(v => v.Producto)
                .WithMany(p => p.valoraciones)
                .HasForeignKey(v => v.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
