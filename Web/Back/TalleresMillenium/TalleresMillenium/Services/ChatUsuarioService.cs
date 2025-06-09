using TalleresMillenium.Models;

namespace TalleresMillenium.Services
{
    public class ChatUsuarioService
    {
        private readonly UnitOfWork _unitOfWork;

        public ChatUsuarioService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<ChatUsuario>> GetAllChatUser(int id)
        {
            return await _unitOfWork.ChatUsuarioRepository.GetAllChatUser(id);
        }
    }
}
