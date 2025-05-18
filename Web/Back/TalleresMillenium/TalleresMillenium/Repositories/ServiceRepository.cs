using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class ServiceRepository : Repository<Servicio, int>
    {
        public ServiceRepository(TalleresMilleniumContext context):base(context) {}

        public async Task<IEnumerable<Servicio>> GetAllService()
        {
            return await GetQueryable().Include(s => s.valoraciones).OrderBy(s => s.Nombre).ToListAsync();
        }
        public async Task<Servicio> GetServiceById(int id)
        {
            return await GetQueryable().Include(s=>s.valoraciones).ThenInclude(v=>v.Usuario).FirstOrDefaultAsync(s=>s.Id==id);
        }
    }
}
