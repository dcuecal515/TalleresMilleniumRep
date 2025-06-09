using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class UserRepository : Repository<Usuario, int>
    {
        public UserRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await GetQueryable()
                .Include(user => user.ChatUsuarios)
                .ThenInclude(user=>user.Chat)
                .Include(user => user.Coches)
                .FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<Usuario> GetByNombreAsync(string nombre)
        {
            return await GetQueryable()
                .Include(user => user.ChatUsuarios)
                .ThenInclude(user => user.Chat)
                .Include(user => user.Coches)
                .FirstOrDefaultAsync(user => user.Name.Equals(nombre));
        }

        public async Task<Usuario> GetByIdAllAsync(int id)
        {
            return await GetQueryable()
                .Include(user => user.ChatUsuarios)
                .ThenInclude(user => user.Chat)
                .Include(user => user.Coches)
                .FirstOrDefaultAsync(user => user.Id == id);
                
        }
        public async Task<Usuario> GetByUserId(int id)
        {
            return await GetQueryable()
                .Include(user => user.ChatUsuarios)
                .ThenInclude(user => user.Chat)
                .Include(user => user.Coches)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<Usuario> GetFullUserById(int id)
        {
            return await GetQueryable()
                .Include(user => user.Coches)
                    .ThenInclude(coche => coche.coche_Servicios)
                        .ThenInclude(coche_servicio => coche_servicio.servicio)
                .FirstOrDefaultAsync(user => user.Id == id);
        }
        public async Task<ICollection<Usuario>> GetAllUser(int id)
        {
            return await GetQueryable()
                 .Where(user => user.Id != id)
                 .ToArrayAsync();
        }
    }
}
