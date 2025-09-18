using System.ComponentModel.DataAnnotations;

namespace HotelSunsetParadise.Models
{
    public class Habitacion
    {
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        [Range(10, 500)]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; } = true;
    }
}
