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
                .Include(x => x.ChatUsuarios)
                    .ThenInclude(u => u.Usuario)
                .FirstOrDefaultAsync(x => x.ChatUsuarios.Any(cu => cu.UsuarioId == userId));
        }

        public async Task<ICollection<Chat>> GetAllChatsAsync()
        {
            return await GetQueryable()
                .Include(x => x.ChatUsuarios)
                    .ThenInclude(u => u.Usuario)
                .ToListAsync();
        }
        public void DeleteRange(IEnumerable<Chat> chats)
        {
            _context.Chats.RemoveRange(chats);
        }
    }
}
