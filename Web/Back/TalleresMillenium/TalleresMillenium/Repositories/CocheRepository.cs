using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class CocheRepository : Repository<Coche, int>
    {
        public CocheRepository(TalleresMilleniumContext context) : base(context) { }

        public async Task<Coche> GetByMatriculaAsync(string matricula)
        {
            return await GetQueryable()
                .Include(x => x.coche_Servicios)
                .ThenInclude(x=>x.servicio)
                .FirstOrDefaultAsync(coche => coche.Matricula.Equals(matricula));
        }
        public async Task<Coche> GetCocheByMatriculaForEmail(string matricula)
        {
            return await GetQueryable()
                .Include(x=>x.Usuario)
                .FirstOrDefaultAsync(coche => coche.Matricula.Equals(matricula));
        }
    }
}
