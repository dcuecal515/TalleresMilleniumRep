using Microsoft.EntityFrameworkCore;

namespace TalleresMillenium.Models
{
    [Index(nameof(Matricula), IsUnique = true)]
    public class Coche
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public string Imagen { get; set; }

        public string Matricula { get; set; }

        public string Fecha_itv { get; set; }

        public string Combustible { get; set; }

        public int ?Kilometraje { get; set; }

        public Usuario Usuario { get; set; }

        public ICollection<Coche_Servicio> coche_Servicios { get; set; }=new List<Coche_Servicio>();


    }
}
