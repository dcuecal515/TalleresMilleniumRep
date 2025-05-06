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
                .Include(x => x.Mensajes)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
