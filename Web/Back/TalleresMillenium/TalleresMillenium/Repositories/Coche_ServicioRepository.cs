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
                .FirstOrDefault(coche_servicio => coche_servicio.CocheId == cocheId && coche_servicio.ServicioId == serviceId && coche_servicio.Estado == "Espera");
            if (coche_Servicio == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
