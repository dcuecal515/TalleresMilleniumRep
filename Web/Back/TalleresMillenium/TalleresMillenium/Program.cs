
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
                    var user1 = new Usuario { Email = "example@gmail.com",Name="Pepe", Password = passwordService.Hash("123456"), Rol = "Admin" };
                    dbContext.Usuarios.Add(user1);
                    dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }

            app.Run();
        }
    }
}
