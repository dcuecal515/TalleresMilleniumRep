using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class ChatUsuarioRepository : Repository<ChatUsuario, int>
    {
        public ChatUsuarioRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<ICollection<ChatUsuario>> GetAllChatUser(int userId)
        {
            return await GetQueryable().Where(x => x.UsuarioId == userId).ToArrayAsync();
        }
    }
}
