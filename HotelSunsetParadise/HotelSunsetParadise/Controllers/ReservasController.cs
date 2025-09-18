using Microsoft.AspNetCore.Mvc;
using HotelSunsetParadise.Models;

namespace HotelSunsetParadise.Controllers
{
    public class ReservasController : Controller
    {
        private static List<Reserva> ReservasList = new List<Reserva>();
        private static List<Cliente> clientes = ClientesController.ClientesList;
        private static List<Habitacion> habitaciones = HabitacionesController.HabitacionesList;

        // ------- LISTAR -------
        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Usuario = usuario;
            return View(ReservasList);
        }

        // ------- CREAR -------
        public IActionResult Create()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Usuario = usuario;

            ViewBag.Clientes = clientes;
            ViewBag.Habitaciones = habitaciones.Where(h => h.Disponible).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(int clienteId, int habitacionId, DateTime entrada, DateTime salida)
        {
            var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
            var habitacion = habitaciones.FirstOrDefault(h => h.Id == habitacionId);

            if (cliente == null || habitacion == null)
                return BadRequest("Cliente o Habitación inválida");

            // VALIDACION
            // FECHAS DE SALIDAS Y ENTRADAS

            if (salida <= entrada)
            {
                ModelState.AddModelError("", "La fecha de salida debe ser posterior a la fecha de entrada.");
                ViewBag.Clientes = clientes;
                ViewBag.Habitaciones = habitaciones.Where(h => h.Disponible).ToList();
                return View();
            }

            // Variable de disa en donde se resta la salida con la entrada para conseguir el numero total de dias de estadia
            // para luego que se multiplique con el precio de la habitacion entonces asi conseguimos e total del precio

            var dias = (salida - entrada).Days;
            var total = dias * habitacion.Precio;

            var reserva = new Reserva
            {
                Id = ReservasList.Count > 0 ? ReservasList.Max(r => r.Id) + 1 : 1,
                Cliente = cliente,
                Habitacion = habitacion,
                FechaEntrada = entrada,
                FechaSalida = salida,
                Total = total
            };

            habitacion.Disponible = false; // <-- cambio la dispo de la habitacion por ocupada cuando se reserva
            ReservasList.Add(reserva);

            return RedirectToAction("Index");
        }

        // ----------- Editar (GET) --------------
        public IActionResult Edit(int id)
        {
            var reserva = ReservasList.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            ViewBag.Clientes = clientes;
            ViewBag.Habitaciones = habitaciones;
            return View(reserva);
        }

        // ----------- Editar (POST) --------------
        [HttpPost]
        public IActionResult Edit(int id, int clienteId, int habitacionId, DateTime entrada, DateTime salida)
        {
            var reserva = ReservasList.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
                return NotFound();

            var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
            var habitacion = habitaciones.FirstOrDefault(h => h.Id == habitacionId);

            if (cliente == null || habitacion == null)
                return BadRequest("Cliente o Habitación inválida");

            if (salida <= entrada)
            {
                ModelState.AddModelError("", "La fecha de salida debe ser posterior a la fecha de entrada.");
                ViewBag.Clientes = clientes;
                ViewBag.Habitaciones = habitaciones;
                return View(reserva);
            }

            var dias = (salida - entrada).Days;

            reserva.Cliente = cliente;
            reserva.Habitacion = habitacion;
            reserva.FechaEntrada = entrada;
            reserva.FechaSalida = salida;
            reserva.Total = dias * habitacion.Precio;

            return RedirectToAction("Index");
        }


        // ------- ELIMINAR -------
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var reserva = ReservasList.FirstOrDefault(r => r.Id == id);
            if (reserva != null)
            {
                // liberar la habitación
                reserva.Habitacion.Disponible = true;
                ReservasList.Remove(reserva);
            }
            return RedirectToAction("Index");
        }
    }
}
