using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class ChatRepository : Repository<Chat, int>
    {
        public ChatRepository(TalleresMilleniumContext context) : base(context) { }


    }
}
