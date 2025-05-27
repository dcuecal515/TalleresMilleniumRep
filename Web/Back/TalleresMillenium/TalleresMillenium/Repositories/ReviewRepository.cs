using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class ReviewRepository : Repository <Valoracion, int>
    {
        public ReviewRepository(TalleresMilleniumContext context) : base(context) { }
    }
}
