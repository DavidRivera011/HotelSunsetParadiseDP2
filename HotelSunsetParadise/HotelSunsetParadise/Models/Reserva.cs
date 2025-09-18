using System;
using System.ComponentModel.DataAnnotations;

namespace HotelSunsetParadise.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public Cliente Cliente { get; set; }

        [Required]
        public Habitacion Habitacion { get; set; }

        [Required]
        public DateTime FechaEntrada { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        public decimal Total { get; set; }
    }
}
