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
    }
}
