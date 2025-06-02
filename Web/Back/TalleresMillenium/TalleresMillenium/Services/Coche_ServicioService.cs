using TalleresMillenium.Models;
namespace TalleresMillenium.Services
{
    public class Coche_ServicioService
    {
        private readonly UnitOfWork _unitOfWork;

        public Coche_ServicioService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Coche_Servicio> GetCoche_ServicioById(int id)
        {
            return await _unitOfWork.Coche_ServicioRepository.GetByIdAsync(id);
        }

        public bool GetIfExistsCoche_Sevicio(int cocheId, int serviceId)
        {
            return _unitOfWork.Coche_ServicioRepository.GetIfExistsCoche_Sevicio(cocheId, serviceId);
        }

        public async Task<Coche_Servicio> InsertCoche_Servicio( Coche_Servicio coche_Servicio)
        {
            Coche_Servicio newCoche_Servicio = await _unitOfWork.Coche_ServicioRepository.InsertAsync(coche_Servicio);
            await _unitOfWork.SaveAsync();
            return newCoche_Servicio;
        }

        public Coche_Servicio GetCoche_ServicioByMYN(string matricula, string nombreServicio)
        {
            Coche_Servicio coche_Servicio = _unitOfWork.Coche_ServicioRepository.GetCoche_ServicioByMYN(matricula, nombreServicio);
            return coche_Servicio;
        }

        public async Task DeleteCoche_Servicio(Coche_Servicio coche_Servicio)
        {
            _unitOfWork.Coche_ServicioRepository.Delete(coche_Servicio);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCoche_Servicio(Coche_Servicio coche_Servicio)
        {
            _unitOfWork.Coche_ServicioRepository.Update(coche_Servicio);
            await _unitOfWork.SaveAsync();
        }
    }
}
