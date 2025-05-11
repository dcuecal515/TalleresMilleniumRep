
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
                        new Servicio{ Nombre="Cambio de aceite y filtro", Descripcion="Protege tu motor y mejora el rendimiento con un cambio de aceite profesional y filtro nuevo. �Tu auto te lo agradecera!", Imagen="/images/cambioaceite.jpg"},
                        new Servicio{ Nombre="Revision y cambio de frenos",Descripcion="Conduce seguro: revisamos y reemplazamos pastillas y discos de freno para maxima seguridad en el camino.", Imagen="/images/cambiofrenos.jpg"},
                        new Servicio{ Nombre="Alineaci�n y balanceo",Descripcion="Evita el desgaste irregular de llantas y mejora la estabilidad de tu auto. �Viaja m�s c�modo y seguro!", Imagen="/images/alineacionbalanceo.webp"},
                        new Servicio{ Nombre="Diagn�stico computarizado del motor",Descripcion="Detectamos fallas con precisi�n gracias a esc�neres avanzados. �Ah�rrate problemas y dinero!", Imagen="/images/diagnostico.webp"},
                        new Servicio{ Nombre="Mantenimiento preventivo",Descripcion="Antic�pate a fallas mayores con revisiones peri�dicas. M�s vida �til para tu auto y menos gastos imprevistos.", Imagen="/images/mantenimientopreventivo.jpg"},
                        new Servicio{ Nombre="Reparaci�n de suspensi�n y direcci�n",Descripcion="Soluciona ruidos, vibraciones y p�rdida de control. Tu auto volver� a sentirse como nuevo.", Imagen="/images/reparacionsuspension.jpg"},
                        new Servicio{ Nombre="Cambio de amortiguadores",Descripcion="Mejora la estabilidad, reduce rebotes y aumenta la comodidad de manejo. �Amortiguadores nuevos, viaje m�s suave!", Imagen="/images/cambioamortiguadores.webp"},
                        new Servicio{ Nombre="Servicio de aire acondicionado",Descripcion="Recupera el aire fr�o en tu veh�culo con nuestro servicio completo de A/C. Ideal para d�as calurosos.", Imagen="/images/servicioaire.jpg"},
                        new Servicio{ Nombre="Revisi�n y reparaci�n del sistema el�ctrico",Descripcion="Solucionamos problemas con luces, arranque, tablero y m�s. �Todo conectado y funcionando como debe!", Imagen="/images/revisionsistemaelectrico.png"},
                        new Servicio{ Nombre="Reparaci�n de transmisi�n",Descripcion="�Tu auto hace ruidos o no cambia bien? Reparamos transmisiones autom�ticas y manuales con garant�a.", Imagen="/images/reparaciontransmision.jpg"},
                        new Servicio{ Nombre="Servicio de escaneo OBDII",Descripcion="Identificamos c�digos de falla y problemas ocultos en minutos. Precisi�n sin adivinanzas.", Imagen="/images/servicioobd.jpg"},
                        new Servicio{ Nombre="Lavado de motor",Descripcion="Limpieza profunda para un motor impecable. Mejora la detecci�n de fugas y la est�tica del veh�culo.", Imagen="/images/ellavadodelmotor.jpg"},
                        new Servicio{ Nombre="Revisi�n y cambio de bater�a",Descripcion="Evita quedarte varado. Verificamos el estado de tu bater�a y la reemplazamos si es necesario.", Imagen="/images/revisionbateria.jpg"},
                        new Servicio{ Nombre="Servicio de inyectores y limpieza del sistema de combustible",Descripcion="Restaura el rendimiento y reduce el consumo limpiando los inyectores de tu motor. �M�s potencia, menos gasto!", Imagen="/images/servicioinyectores.jpg"},
                        new Servicio{ Nombre="Revisi�n t�cnica pre-compra",Descripcion="Aseg�rate de hacer una buena compra. Inspeccionamos el veh�culo usado antes de que lo adquieras.", Imagen="/images/revisionpreventa.jpg"}
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
