using Microsoft.AspNetCore.Mvc;
using HotelSunsetParadise.Models;

namespace HotelSunsetParadise.Controllers
{
    public class HabitacionesController : Controller
    {

        public static List<Habitacion> HabitacionesList = new List<Habitacion>();

        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Usuario = usuario;
            return View(HabitacionesList);
        }

        public IActionResult Create()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Usuario = usuario;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Habitacion habitacion)
        {

            // Validacion de numero de habitacion
            if (HabitacionesList.Any(h => h.Numero == habitacion.Numero))
            {
                ModelState.AddModelError("Numero", "El número de habitación ya existe.");
                return View(habitacion);
            }

            if (ModelState.IsValid)
            {
                habitacion.Id = HabitacionesList.Count > 0 ? HabitacionesList.Max(h => h.Id) + 1 : 1;
                HabitacionesList.Add(habitacion);
                return RedirectToAction("Index");
            }
            return View(habitacion);
        }

        // ----------- Editar (GET) --------------
        public IActionResult Edit(int id)
        {
            var habitacion = HabitacionesList.FirstOrDefault(h => h.Id == id);
            if (habitacion == null)
            {
                return NotFound();
            }
            return View(habitacion);
        }

        // ----------- Editar (POST) --------------
        [HttpPost]
        public IActionResult Edit(Habitacion habitacion)
        {
            var h = HabitacionesList.FirstOrDefault(x => x.Id == habitacion.Id);
            if (h == null)
                return NotFound();

            // Validacion de numero por si se ingresa el mismo en otra habitacion
            if (HabitacionesList.Any(x => x.Numero == habitacion.Numero && x.Id != habitacion.Id))
            {
                ModelState.AddModelError("Numero", "El número de habitación ya está en uso por otra habitación.");
                return View(habitacion);
            }

            if (ModelState.IsValid)
            {
                h.Numero = habitacion.Numero;
                h.Tipo = habitacion.Tipo;
                h.Precio = habitacion.Precio;
                h.Disponible = habitacion.Disponible;

                return RedirectToAction("Index");
            }

            return View(habitacion);
        }



        // ----------- Eliminar --------------
        public IActionResult Delete(int id) // GET
        {
            var habitacion = HabitacionesList.FirstOrDefault(h => h.Id == id);
            if (habitacion == null)
            {
                return NotFound();
            }
            return View(habitacion);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id) // POST
        {
            var habitacion = HabitacionesList.FirstOrDefault(h => h.Id == id);
            if (habitacion != null)
            {
                HabitacionesList.Remove(habitacion);
            }
            return RedirectToAction("Index");
        }
    }
}
