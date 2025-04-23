
using Microsoft.Extensions.Options;
using TalleresMillenium.Models;
using TalleresMillenium.Services;

namespace TalleresMillenium
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
            builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<Settings>>().Value);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<TalleresMilleniumContext>();
            builder.Services.AddScoped<UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            PasswordService passwordService = new PasswordService();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                TalleresMilleniumContext dbContext = scope.ServiceProvider.GetService<TalleresMilleniumContext>();
                if (dbContext.Database.EnsureCreated())
                {
                    var user1 = new Usuario { Email = "example@gmail.com", Password = passwordService.Hash("123456"), Rol = "Admin" };
                    dbContext.Usuarios.Add(user1);
                    dbContext.SaveChanges();
                }
                dbContext.SaveChanges();
            }

            app.Run();
        }
    }
}
