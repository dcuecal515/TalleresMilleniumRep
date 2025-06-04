using Microsoft.EntityFrameworkCore;
using TalleresMillenium.Models;
using TalleresMillenium.Repositories.Base;

namespace TalleresMillenium.Repositories
{
    public class Coche_ServicioRepository : Repository<Coche_Servicio, int>
    {
        public Coche_ServicioRepository(TalleresMilleniumContext context) : base(context) { }

        public bool GetIfExistsCoche_Sevicio(int cocheId, int serviceId)
        {
            Coche_Servicio coche_Servicio = GetQueryable()
                .FirstOrDefault(coche_servicio => coche_servicio.CocheId == cocheId && coche_servicio.ServicioId == serviceId && (coche_servicio.Estado == "Espera" || coche_servicio.Estado == "Reservado"));
           
            if (coche_Servicio == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
        public Coche_Servicio GetCoche_ServicioByMYN(string matricula, string nombreServicio)
        {
            Coche_Servicio coche_Servicio = GetQueryable()
                .FirstOrDefault(coche_servicio => coche_servicio.coche.Matricula == matricula && coche_servicio.servicio.Nombre == nombreServicio);
            return coche_Servicio;
        }
        public async Task<ICollection<Coche_Servicio>> getallCoche_servicio()
        {
            return await GetQueryable().Where(cs=>cs.Estado=="Reservado" || cs.Estado=="Aceptado").OrderBy(cs=>cs.Fecha).Include(cs=>cs.coche).Include(cs=>cs.servicio).ToListAsync();
        }
    }
}
