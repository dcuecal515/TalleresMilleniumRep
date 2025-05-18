using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class ChatRepository : Repository<Chat, int>
    {
        public ChatRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<Chat> GetChatByUserIdAsync(int userId)
        {
            return await GetQueryable()
                .Include(x => x.Usuarios)
                .FirstOrDefaultAsync(x => x.Usuarios.Any(u => u.Id == userId));
        }

        public async Task<ICollection<Chat>> GetAllChatsAsync()
        {
            return await GetQueryable()
                .Include(x => x.Usuarios)
                .ToListAsync();
        }
    }
}
