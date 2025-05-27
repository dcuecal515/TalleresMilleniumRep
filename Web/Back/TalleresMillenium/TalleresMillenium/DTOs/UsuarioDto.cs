namespace TalleresMillenium.DTOs
{
    public class UsuarioDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Imagen { get; set; }
        public List<CocheDto> Coches{ get; set; }
    }
}
