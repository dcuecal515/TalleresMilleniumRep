namespace TalleresMillenium.DTOs
{
    public class SignUpDto
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public string contrasena { get; set; }
        public IFormFile imagenPerfil  { get; set; }
    }
}
