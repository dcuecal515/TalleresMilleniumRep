using TalleresMillenium.Models;

namespace TalleresMillenium
{
    public class WSHelper
    {
        private readonly UnitOfWork _unitOfWork;

        public WSHelper(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Usuario> GetUserById(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<Usuario> GetUserByNombre(string nombre)
        {
            return await _unitOfWork.UserRepository.GetByNombreAsync(nombre);
        }
    }
}
