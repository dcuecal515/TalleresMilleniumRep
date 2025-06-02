using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class MensajeRepository : Repository<Mensaje, int>
    {
        public MensajeRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<ICollection<Mensaje>> GetAllMensajesByChatId(int chatId)
        {

            return await GetQueryable()
                .Where(m => m.ChatId == chatId)
                .ToListAsync();
        }
    }
}
