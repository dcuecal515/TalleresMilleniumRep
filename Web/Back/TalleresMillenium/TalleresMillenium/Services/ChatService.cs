using System;
using TalleresMillenium.DTOs;
using TalleresMillenium.Models;
namespace TalleresMillenium.Services
{
    public class ChatService
    {
        private readonly UnitOfWork _unitOfWork;

        public ChatService(UnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ChatDto>> GetAllChatsAdmin()
        {
            ICollection<Chat> chats = await _unitOfWork.ChatRepository.GetAllChatsAsync();

            if(chats.Count == 0)
            {
                return null;
            }

            List<ChatDto> chatsDtos = new List<ChatDto>();
            
            
            foreach (var chat in chats)
            {
                ChatUsuario chatUsuario = chat.ChatUsuarios.FirstOrDefault(u=>u.Usuario.Rol!="Admin");
                Usuario user = chatUsuario.Usuario;
                ICollection<Mensaje> mensajes = await _unitOfWork.MensajeRepository.GetAllMensajesByChatId(chat.Id);
                List<MensajeDto> mensajeDtos = new List<MensajeDto>();

                foreach (var mensaje in mensajes)
                {
                    Usuario userMensaje = await _unitOfWork.UserRepository.GetByIdAsync(mensaje.UsuarioId);

                    MensajeDto mensajeDto = new MensajeDto
                    {
                        UserName = userMensaje.Name,
                        Texto = mensaje.Texto
                    };

                    mensajeDtos.Add(mensajeDto);
                }

                ChatDto chatDto = new ChatDto
                {
                    Username = user.Name,
                    Mensajes = mensajeDtos
                };

                chatsDtos.Add(chatDto);
            }


            return chatsDtos;
        }

        public async Task<List<ChatDto>> GetAllChatsUser(int userId)
        {
            Chat chat = await _unitOfWork.ChatRepository.GetChatByUserIdAsync(userId);

            if(chat == null)
            {
                return null;
            }

            List<ChatDto> chatsDtos = new List<ChatDto>();

            ChatUsuario chatUsuario = chat.ChatUsuarios.FirstOrDefault(u => u.Usuario.Rol != "Admin");
            Usuario user = chatUsuario.Usuario;
            ICollection<Mensaje> mensajes = await _unitOfWork.MensajeRepository.GetAllMensajesByChatId(chat.Id);
            List<MensajeDto> mensajeDtos = new List<MensajeDto>();

            foreach (var mensaje in mensajes)
            {
                Usuario userMensaje = await _unitOfWork.UserRepository.GetByIdAsync(mensaje.UsuarioId);

                MensajeDto mensajeDto = new MensajeDto
                {
                    UserName = userMensaje.Name,
                    Texto = mensaje.Texto
                };

                mensajeDtos.Add(mensajeDto);
            }

            ChatDto chatDto = new ChatDto
            {
                Username = user.Name,
                Mensajes = mensajeDtos
            };

            chatsDtos.Add(chatDto);

            return chatsDtos;
        }
        public async Task DeleteManyChats(IEnumerable<int> ids)
        {
            ICollection<Chat> chats = await _unitOfWork.ChatRepository.GetAllAsync();
            List<Chat> chatsaeliminar = new List<Chat>();
            foreach (var chat in chats)
            {
                if (ids.Contains(chat.Id))
                {
                    chatsaeliminar.Add(chat);
                }
            }
            _unitOfWork.ChatRepository.DeleteRange(chatsaeliminar);
            await _unitOfWork.SaveAsync();
        }
    }
}
