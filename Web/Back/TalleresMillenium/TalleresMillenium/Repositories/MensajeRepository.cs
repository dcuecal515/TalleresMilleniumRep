using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class MensajeRepository : Repository<Mensaje, int>
    {
        public MensajeRepository(TalleresMilleniumContext context) : base(context) { }


    }
}
