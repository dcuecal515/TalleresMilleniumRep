using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class CocheService
    {
        private readonly UnitOfWork _unitOfWork;

        public CocheService(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<Coche> InsertCocheAsync(Coche coche)
        {
            Coche updCoche = await _unitOfWork.CocheRepository.InsertAsync(coche);
            await _unitOfWork.SaveAsync();
            return updCoche;
        }

        public async Task<Coche> GetCocheByMatricula(string matricula)
        {
            Coche coche = await _unitOfWork.CocheRepository.GetByMatriculaAsync(matricula);
            return coche;
        }
    }
}
