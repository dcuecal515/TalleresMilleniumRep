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
            Servicio servicio = await GetQueryable().Include(servicio=>servicio.valoraciones).ThenInclude(valoracion=> valoracion.Usuario).FirstOrDefaultAsync(s=>s.Id==id);
            return servicio;
        }
        public async Task<IEnumerable<Servicio>> GetAllServiceFull()
        {
            return await GetQueryable().OrderBy(p => p.Nombre).ToListAsync();
        }
    }
}
