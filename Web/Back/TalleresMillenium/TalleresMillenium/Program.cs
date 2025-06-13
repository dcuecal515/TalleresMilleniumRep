
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TalleresMillenium.Models;
using TalleresMillenium.Services;
using TalleresMillenium.Mappers;
using System.Text.Json.Serialization;

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

            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
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
            builder.Services.AddScoped<ServiceService>();
            builder.Services.AddScoped<WSHelper>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddScoped<ReviewMapper>();
            builder.Services.AddScoped<ReviewService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<Coche_ServicioMapper>();
            builder.Services.AddScoped<CocheService>();
            builder.Services.AddScoped<Coche_ServicioService>();
            builder.Services.AddScoped<ChatUsuarioService>();
            builder.Services.AddScoped<EmailService>();


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
            app.UseWebSockets();

            app.UseMiddleware<WebSocketMiddleware>();
          
            app.UseHttpsRedirection();

            app.UseRouting();

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
                        new Servicio{ Nombre="Alineacion y balanceo",Descripcion="Evita el desgaste irregular de llantas y mejora la estabilidad de tu auto. ¡Viaja mas comodo y seguro!", Imagen="/images/alineacionbalanceo.webp"},
                        new Servicio{ Nombre="Diagnostico computarizado del motor",Descripcion="Detectamos fallas con precision gracias a escaneres avanzados. ¡Ahorrate problemas y dinero!", Imagen="/images/diagnostico.webp"},
                        new Servicio{ Nombre="Mantenimiento preventivo",Descripcion="Anticipate a fallas mayores con revisiones periodicas. Mas vida util para tu auto y menos gastos imprevistos.", Imagen="/images/mantenimientopreventivo.jpg"},
                        new Servicio{ Nombre="Reparacion de suspension y direccion",Descripcion="Soluciona ruidos, vibraciones y perdida de control. Tu auto volvera a sentirse como nuevo.", Imagen="/images/reparacionsuspension.png"},
                        new Servicio{ Nombre="Cambio de amortiguadores",Descripcion="Mejora la estabilidad, reduce rebotes y aumenta la comodidad de manejo. ¡Amortiguadores nuevos, viaje mas suave!", Imagen="/images/cambioamortiguadores.webp"},
                        new Servicio{ Nombre="Servicio de aire acondicionado",Descripcion="Recupera el aire frio en tu vehiculo con nuestro servicio completo de A/C. Ideal para dias calurosos.", Imagen="/images/servicioaire.jpg"},
                        new Servicio{ Nombre="Revision y reparacion del sistema electrico",Descripcion="Solucionamos problemas con luces, arranque, tablero y mas. ¡Todo conectado y funcionando como debe!", Imagen="/images/revisionsistemaelectrico.png"},
                        new Servicio{ Nombre="Reparacion de transmision",Descripcion="¿Tu auto hace ruidos o no cambia bien? Reparamos transmisiones automaticas y manuales con garantia.", Imagen="/images/reparaciontransmision.jpg"},
                        new Servicio{ Nombre="Servicio de escaneo OBDII",Descripcion="Identificamos codigos de falla y problemas ocultos en minutos. Precision sin adivinanzas.", Imagen="/images/servicioobd.jpg"},
                        new Servicio{ Nombre="Lavado de motor",Descripcion="Limpieza profunda para un motor impecable. Mejora la deteccion de fugas y la estetica del vehiculo.", Imagen="/images/ellavadodelmotor.jpg"},
                        new Servicio{ Nombre="Revision y cambio de bateria",Descripcion="Evita quedarte varado. Verificamos el estado de tu bateria y la reemplazamos si es necesario.", Imagen="/images/revisionbateria.jpg"},
                        new Servicio{ Nombre="Servicio de inyectores y limpieza del sistema de combustible",Descripcion="Restaura el rendimiento y reduce el consumo limpiando los inyectores de tu motor. ¡Mas potencia, menos gasto!", Imagen="/images/servicioinyectores.jpg"},
                        new Servicio{ Nombre="Revision tecnica pre-compra",Descripcion="Asegurate de hacer una buena compra. Inspeccionamos el vehiculo usado antes de que lo adquieras.", Imagen="/images/revisionpreventa.jpg"}
                    };
                    var productos = new List<Producto>
{
                        new Producto{Nombre = "Aceites de motor", Descripcion = "Mantén tu motor lubricado y protegido con aceites de alta calidad. Variedad para cada tipo de vehículo.", Imagen = "/images/aceitemotor.jpg", Disponible = "Disponible"},
                        new Producto{Nombre = "Filtros de aceite, aire y gasolina", Descripcion = "Mejora el rendimiento del motor filtrando impurezas. Cambiar filtros es esencial para el cuidado del auto.", Imagen = "/images/filtros.jpg", Disponible = "No disponible"},
                        new Producto{Nombre = "Pastillas y discos de freno", Descripcion = "Frenado seguro con piezas de alta durabilidad. Tenemos repuestos para múltiples marcas y modelos.", Imagen = "/images/discofreno.jpg", Disponible = "Disponible"},
                        new Producto{Nombre = "Amortiguadores", Descripcion = "Viaja cómodo y estable. Contamos con amortiguadores de marcas reconocidas y garantía de fábrica.", Imagen = "/images/amortiguadores.webp", Disponible = "No disponible"},
                        new Producto{Nombre = "Baterías para auto", Descripcion = "Energía confiable para tu vehículo. Venta e instalación de baterías con prueba incluida.", Imagen = "/images/bateriacoche.png", Disponible = "Disponible"},
                        new Producto{Nombre = "Limpiaparabrisas", Descripcion = "Visibilidad clara en lluvia o polvo. Escobillas de alto rendimiento para cada temporada.", Imagen = "/images/limpiaparabrisas.jpg", Disponible = "No disponible"},
                        new Producto{Nombre = "Refrigerante / Anticongelante", Descripcion = "Protege tu motor del sobrecalentamiento con nuestros refrigerantes premium.", Imagen = "/images/refrigerante.jpg", Disponible = "No disponible"},
                        new Producto{Nombre = "Aditivos para motor y combustible", Descripcion = "Mejora el rendimiento, reduce el desgaste y limpia tu sistema. ¡Una pequeña inversión que hace la diferencia!", Imagen = "/images/aditivos.webp", Disponible = "Disponible"},
                        new Producto{Nombre = "Bombillas y faros", Descripcion = "Luz potente y segura. Cambia tus faros fundidos con nuestras bombillas homologadas.", Imagen = "/images/bombillas.jpg", Disponible = "No disponible"},
                        new Producto{Nombre = "Correas de distribución y accesorios", Descripcion = "Evita daños graves al motor reemplazando las correas a tiempo. Tenemos las adecuadas para tu auto.", Imagen = "/images/correaservicio.webp", Disponible = "Disponible"},
                        new Producto{Nombre = "Sensores (oxígeno, temperatura, etc.)", Descripcion = "Repuestos originales para un funcionamiento óptimo del motor. Diagnóstico e instalación.", Imagen = "/images/sensor.jpg", Disponible = "No disponible"},
                        new Producto{Nombre = "Llantas / neumáticos", Descripcion = "Mejora el agarre y la seguridad con neumáticos nuevos. Contamos con marcas reconocidas a buen precio.", Imagen = "/images/llantas.jpg", Disponible = "Disponible"},
                        new Producto{Nombre = "Cables de bujías y bobinas", Descripcion = "Garantiza una chispa fuerte y estable. Mejora el encendido y rendimiento del motor.", Imagen = "/images/cablesbujias.jpg", Disponible = "Disponible"},
                        new Producto{Nombre = "Líquido de frenos", Descripcion = "Elemento esencial para un frenado eficaz. Lo tenemos en stock y te lo cambiamos al instante.", Imagen = "/images/liquidofrenos.jpg", Disponible = "No disponible"},
                        new Producto{Nombre = "Kit de herramientas básicas para el auto", Descripcion = "Ideal para emergencias. Incluye llaves, gato, maneral y más. ¡Todo conductor debería tener uno!", Imagen = "/images/kit.jpg", Disponible = "Disponible"}
                        };
                    var user1 = new Usuario { Email = "example@gmail.com",Name="Pepe", Password = passwordService.Hash("123456"), Rol = "Admin" , Imagen= "/images/perfilDefect.webp" };
                    dbContext.Usuarios.Add(user1);
                    dbContext.Servicios.AddRange(servicios);
                    dbContext.Productos.AddRange(productos);
                    dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }

            app.Run();
        }
    }
}
