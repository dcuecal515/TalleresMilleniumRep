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

        public async Task<ChatDto[]> GetAllChatsAdmin()
        {
            ICollection<Chat> chats = await _unitOfWork.ChatRepository.GetAllChatsAsync();
            ChatDto[] chatsDtos = new ChatDto[chats.Count];
            
            
            foreach (var chat in chats)
            {
                Usuario user = chat.Usuarios.FirstOrDefault(u => u.Rol != "Admin");
                ICollection<Mensaje> mensajes = await _unitOfWork.MensajeRepository.GetAllMensajesByChatId(chat.Id);
                MensajeDto[] mensajeDtos = new MensajeDto[mensajes.Count];

                foreach (var mensaje in mensajes)
                {
                    Usuario userMensaje = await _unitOfWork.UserRepository.GetByIdAsync(mensaje.UserId);

                    MensajeDto mensajeDto = new MensajeDto
                    {
                        UserName = userMensaje.Name,
                        Texto = mensaje.Texto
                    };

                    mensajeDtos.Append(mensajeDto);
                }

                ChatDto chatDto = new ChatDto
                {
                    Username = user.Name,
                    Mensajes = mensajeDtos
                };

                chatsDtos.Append(chatDto);
            }


            return chatsDtos;
        }

        public async Task<ChatDto[]> GetAllChatsUser(int userId)
        {
            Chat chat = await _unitOfWork.ChatRepository.GetChatByUserIdAsync(userId);

            ChatDto[] chatsDtos = [];

            Usuario user = chat.Usuarios.FirstOrDefault(u => u.Rol != "Admin");
            ICollection<Mensaje> mensajes = await _unitOfWork.MensajeRepository.GetAllMensajesByChatId(chat.Id);
            MensajeDto [] mensajeDtos = new MensajeDto [mensajes.Count];

            foreach (var mensaje in mensajes)
            {
                Usuario userMensaje = await _unitOfWork.UserRepository.GetByIdAsync(mensaje.UserId);

                MensajeDto mensajeDto = new MensajeDto
                {
                    UserName = userMensaje.Name,
                    Texto = mensaje.Texto
                };

                mensajeDtos.Append(mensajeDto);
            }

            ChatDto chatDto = new ChatDto
            {
                Username = user.Name,
                Mensajes = mensajeDtos
            };

            chatsDtos.Append(chatDto);

            return chatsDtos;
        }
    }
}
