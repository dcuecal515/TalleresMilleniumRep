
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TalleresMillenium.Models;
using TalleresMillenium.Services;
using TalleresMillenium.Mappers;

namespace TalleresMillenium
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            // Add services to the container.
            builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<Settings>>().Value);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            builder.Services.AddScoped<TalleresMilleniumContext>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<UserMapper>();
            builder.Services.AddScoped<CocheMapper>();
            builder.Services.AddSingleton<WebSocketService>();

            builder.Services.AddCors(
                options =>
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                        ;
                    })
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            });

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            PasswordService passwordService = new PasswordService();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                TalleresMilleniumContext dbContext = scope.ServiceProvider.GetService<TalleresMilleniumContext>();
                if (dbContext.Database.EnsureCreated())
                {
                    var servicios = new List<Servicio>
                    {
                        new Servicio{ Nombre="Cambio de aceite y filtro", Descripcion="Protege tu motor y mejora el rendimiento con un cambio de aceite profesional y filtro nuevo. ¡Tu auto te lo agradecera!", Imagen="/images/cambioaceite.jpg"},
                        new Servicio{ Nombre="Revision y cambio de frenos",Descripcion="Conduce seguro: revisamos y reemplazamos pastillas y discos de freno para maxima seguridad en el camino.", Imagen="/images/cambiofrenos.jpg"},
                        new Servicio{ Nombre="Alineación y balanceo",Descripcion="Evita el desgaste irregular de llantas y mejora la estabilidad de tu auto. ¡Viaja más cómodo y seguro!", Imagen="/images/alineacionbalanceo.webp"},
                        new Servicio{ Nombre="Diagnóstico computarizado del motor",Descripcion="Detectamos fallas con precisión gracias a escáneres avanzados. ¡Ahórrate problemas y dinero!", Imagen="/images/diagnostico.webp"},
                        new Servicio{ Nombre="Mantenimiento preventivo",Descripcion="Anticípate a fallas mayores con revisiones periódicas. Más vida útil para tu auto y menos gastos imprevistos.", Imagen="/images/mantenimientopreventivo.jpg"},
                        new Servicio{ Nombre="Reparación de suspensión y dirección",Descripcion="Soluciona ruidos, vibraciones y pérdida de control. Tu auto volverá a sentirse como nuevo.", Imagen="/images/reparacionsuspension.jpg"},
                        new Servicio{ Nombre="Cambio de amortiguadores",Descripcion="Mejora la estabilidad, reduce rebotes y aumenta la comodidad de manejo. ¡Amortiguadores nuevos, viaje más suave!", Imagen="/images/cambioamortiguadores.webp"},
                        new Servicio{ Nombre="Servicio de aire acondicionado",Descripcion="Recupera el aire frío en tu vehículo con nuestro servicio completo de A/C. Ideal para días calurosos.", Imagen="/images/servicioaire.jpg"},
                        new Servicio{ Nombre="Revisión y reparación del sistema eléctrico",Descripcion="Solucionamos problemas con luces, arranque, tablero y más. ¡Todo conectado y funcionando como debe!", Imagen="/images/revisionsistemaelectrico.png"},
                        new Servicio{ Nombre="Reparación de transmisión",Descripcion="¿Tu auto hace ruidos o no cambia bien? Reparamos transmisiones automáticas y manuales con garantía.", Imagen="/images/reparaciontransmision.jpg"},
                        new Servicio{ Nombre="Servicio de escaneo OBDII",Descripcion="Identificamos códigos de falla y problemas ocultos en minutos. Precisión sin adivinanzas.", Imagen="/images/servicioobd.jpg"},
                        new Servicio{ Nombre="Lavado de motor",Descripcion="Limpieza profunda para un motor impecable. Mejora la detección de fugas y la estética del vehículo.", Imagen="/images/ellavadodelmotor.jpg"},
                        new Servicio{ Nombre="Revisión y cambio de batería",Descripcion="Evita quedarte varado. Verificamos el estado de tu batería y la reemplazamos si es necesario.", Imagen="/images/revisionbateria.jpg"},
                        new Servicio{ Nombre="Servicio de inyectores y limpieza del sistema de combustible",Descripcion="Restaura el rendimiento y reduce el consumo limpiando los inyectores de tu motor. ¡Más potencia, menos gasto!", Imagen="/images/servicioinyectores.jpg"},
                        new Servicio{ Nombre="Revisión técnica pre-compra",Descripcion="Asegúrate de hacer una buena compra. Inspeccionamos el vehículo usado antes de que lo adquieras.", Imagen="/images/revisionpreventa.jpg"}
                    };
                    var user1 = new Usuario { Email = "example@gmail.com",Name="Pepe", Password = passwordService.Hash("123456"), Rol = "Admin" };
                    dbContext.Usuarios.Add(user1);
                    dbContext.Servicios.AddRange(servicios);
                    dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }

            app.Run();
        }
    }
}
