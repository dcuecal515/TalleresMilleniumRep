namespace TalleresMillenium.DTOs
{
    public class SignUpDto
    {
        public string nombre { get; set; }
        public string email { get; set; }
        public string contrasena { get; set; }
        public IFormFile imagenPerfil  { get; set; }
        public string matricula { get; set; }
        public string tipo_vehiculo { get; set; }
        public string fecha_ITV { get; set; }
        public string tipo_combustible { get; set; }
        public IFormFile imagenFT {  get; set; }
    }
}
