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

        public async Task<Chat> GetChatByUserId(int id)
        {
            return await _unitOfWork.ChatRepository.GetChatByUserIdAsync(id);
        }

        public async Task InsertChatAsync(Chat chat) {
            await _unitOfWork.ChatRepository.InsertAsync(chat);
            await _unitOfWork.SaveAsync();
        }

        public async Task InsertMensajeAsync(Mensaje mensaje)
        {
            await _unitOfWork.MensajeRepository.InsertAsync(mensaje);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateChatAsync(Chat chat)
        {
            _unitOfWork.ChatRepository.Update(chat);
            await _unitOfWork.SaveAsync();
        }
    }
}
