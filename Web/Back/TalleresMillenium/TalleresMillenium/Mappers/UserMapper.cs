using TalleresMillenium.DTOs;
using TalleresMillenium.Models;

namespace TalleresMillenium.Mappers
{
    public class UserMapper
    {
        public Usuario toEntity(SignUpDto userDto)
        {
            return new Usuario
            {
                Name = userDto.nombre,
                Email = userDto.email
            };
        }

        public UsuarioDto toDto(Usuario user) {
            return new UsuarioDto
            {
                Email = user.Email,
                Name = user.Name,
                Imagen = user.Imagen
            };    
        }
        public List<ListuserDto> ListtoDto(List<Usuario> users)
        {
            List< ListuserDto > listuserDtos = new List< ListuserDto >();
            foreach (Usuario user in users)
            {
                ListuserDto listuserDto = new ListuserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Imagen = user.Imagen,
                    Rol = user.Rol
                };
                listuserDtos.Add(listuserDto);
            }
            return listuserDtos;
        }
    }
}
