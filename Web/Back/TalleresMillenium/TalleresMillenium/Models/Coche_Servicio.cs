namespace TalleresMillenium.Models
{
    public class Coche_Servicio
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public int CocheId { get; set; }
        public Coche coche { get; set; }
        public int ServicioId { get; set; }
        public Servicio servicio { get; set; }

    }
}
