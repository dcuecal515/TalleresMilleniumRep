using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace TalleresMillenium.Repositories
{
    public class ReviewRepository : Repository <Valoracion, int>
    {
        public ReviewRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<Valoracion> GetExixtsServiceReview(int ServicioId, int id)
        {
            return await GetQueryable().FirstOrDefaultAsync(valoracion => valoracion.ServicioId == ServicioId && valoracion.UsuarioId == id);
        }
        public async Task<Valoracion> GetExixtsProductReview(int ServicioId, int id)
        {
            return await GetQueryable().FirstOrDefaultAsync(valoracion => valoracion.ProductoId == ServicioId && valoracion.UsuarioId == id);
        }
    }
}
