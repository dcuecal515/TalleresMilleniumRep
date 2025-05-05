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
            return await GetQueryable().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }

        public async Task<Usuario> GetByNombreAsync(string nombre)
        {
            return await GetQueryable().FirstOrDefaultAsync(user => user.Name.Equals(nombre));
        }
    }
}
