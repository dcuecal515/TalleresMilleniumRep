using System.Net.WebSockets;
using System.Text.Json;
using TalleresMillenium.Models;
using TalleresMillenium.DTOs;

namespace TalleresMillenium.Services
{
    public class WebSocketService
    {
        private readonly IServiceProvider _serviceProvider;

        public WebSocketService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly List<WebSocketHandler> _handlers = new List<WebSocketHandler>();

        // Semáforo para controlar el acceso a la lista de WebSocketHandler
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public async Task HandleAsync(WebSocket webSocket, Usuario user)
        {
            WebSocketHandler handler = await AddWebsocketAsync(webSocket, user.Id);
            await handler.HandleAsync();
        }

        private async Task<WebSocketHandler> AddWebsocketAsync(WebSocket webSocket, int id)
        {
            // Esperamos a que haya un hueco disponible
            await _semaphore.WaitAsync();

            // Sección crítica
            // Creamos un nuevo WebSocketHandler, nos suscribimos a sus eventos y lo añadimos a la lista
            WebSocketHandler handler = new WebSocketHandler(id, webSocket);
            handler.MessageReceived += OnMessageReceivedAsync;
            _handlers.Add(handler);

            // Liberamos el semáforo
            _semaphore.Release();

            return handler;
        }

        private async Task OnMessageReceivedAsync(WebSocketHandler userHandler, string message)
        {
            // Lista donde guardar las tareas de envío de mensajes
            List<Task> tasks = new List<Task>();
            // Guardamos una copia de los WebSocketHandler para evitar problemas de concurrencia
            WebSocketHandler [] handlers = _handlers.ToArray();

            Console.WriteLine(message);

            message = message.Substring(1, message.Length - 2);
            message = message.Replace("\\", "");

            MensajeRecividoDto mensajeRecivido = JsonSerializer.Deserialize<MensajeRecividoDto>(message);

            if(mensajeRecivido.TypeMessage.Equals("mensaje a admin"))
            {
                string nombre_admin = "Pepe";
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _wsHelper = scope.ServiceProvider.GetRequiredService<WSHelper>();
                    Usuario user = await _wsHelper.GetUserById(userHandler.Id);
                    Usuario user2 = await _wsHelper.GetUserByNombre(nombre_admin);

                    Usuario[] admins = await _wsHelper.GetAllAdmins();

                    foreach (var admin in admins) {
                        foreach (WebSocketHandler handler in handlers)
                        {
                            if (handler.Id == admin.Id)
                            {
                                WebsocketMessageDto outMessage = new WebsocketMessageDto
                                {
                                    Message = "Te llego un mensaje",
                                    Texto = mensajeRecivido.Identifier,
                                    UserName = user.Name
                                };
                                string messageToSend = JsonSerializer.Serialize(outMessage, JsonSerializerOptions.Web);
                                tasks.Add(handler.SendAsync(messageToSend));
                            }
                        }

                        if (user.ChatUsuarios.Count == 0) 
                        {
                            Chat chat = new Chat
                            {
                                ChatUsuarios = new List<ChatUsuario>
                                {
                                    new ChatUsuario { UsuarioId = user.Id },
                                    new ChatUsuario { UsuarioId = user2.Id }
                                }
                            };

                        chat = await _wsHelper.InsertChatAsync(chat);


                            Mensaje mensaje = new Mensaje
                            {
                                UsuarioId = user.Id,
                                ChatId= chat.Id,
                                Texto = mensajeRecivido.Identifier
                            };
                            
                 
                        await _wsHelper.InsertMensajeAsync(mensaje);
                    } else
                    {
                            Chat chat = await _wsHelper.GetChatByUserId(user.Id);
                            Mensaje mensaje = new Mensaje
                            {
                                UsuarioId = user.Id,
                                ChatId = chat.Id,
                                Texto = mensajeRecivido.Identifier
                            };
                            await _wsHelper.InsertMensajeAsync(mensaje);
                        }
                    }
                    
                }
            }

            if (mensajeRecivido.TypeMessage.Equals("mensaje a otro"))
            {
                string nombre_cliente = mensajeRecivido.Identifier;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _wsHelper = scope.ServiceProvider.GetRequiredService<WSHelper>();
                    Usuario user = await _wsHelper.GetUserById(userHandler.Id);
                    Usuario user2 = await _wsHelper.GetUserByNombre(nombre_cliente);
                    if (user2 != null)
                    {
                        foreach (WebSocketHandler handler in handlers)
                        {
                            if (handler.Id == user2.Id)
                            {
                                WebsocketMessageDto outMessage = new WebsocketMessageDto
                                {
                                    Message = "Te llego un mensaje de admin",
                                    Texto = mensajeRecivido.Identifier2,
                                    UserName = user.Name
                                };
                                string messageToSend = JsonSerializer.Serialize(outMessage, JsonSerializerOptions.Web);
                                tasks.Add(handler.SendAsync(messageToSend));
                            }
                        }
                        Chat chat = await _wsHelper.GetChatByUserId(user2.Id);
                        Mensaje mensaje = new Mensaje
                        {
                            UsuarioId = user.Id,
                            ChatId = chat.Id,
                            Texto = mensajeRecivido.Identifier2
                        };
                        await _wsHelper.InsertMensajeAsync(mensaje);
                    }
                }
            }

            await Task.WhenAll(tasks);
        }
    }
}
