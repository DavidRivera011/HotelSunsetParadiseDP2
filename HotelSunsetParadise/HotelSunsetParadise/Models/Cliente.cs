using System.ComponentModel.DataAnnotations;

namespace HotelSunsetParadise.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 9)]
        public string DUI { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }
    }
}
