using Microsoft.AspNetCore.Mvc;
using HotelSunsetParadise.Models;

namespace HotelSunsetParadise.Controllers
{
    public class ClientesController : Controller
    {
        public static List<Cliente> ClientesList = new List<Cliente>();

        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if (usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Usuario = usuario;
            return View(ClientesList);
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
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Id = ClientesList.Count + 1;
                ClientesList.Add(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // ----------- Editar (GET) --------------
        public IActionResult Edit(int id)
        {
            var cliente = ClientesList.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // ----------- Editar (POST) --------------
        [HttpPost]
        public IActionResult Edit(Cliente cliente)
        {
            var c = ClientesList.FirstOrDefault(x => x.Id == cliente.Id);
            if (c == null)
                return NotFound();

            if (ClientesList.Any(x => x.DUI == cliente.DUI && x.Id != cliente.Id))
            {
                ModelState.AddModelError("DUI", "El DUI del cliente ya está en uso por otro cliente.");
                return View(cliente);
            }

            if (ModelState.IsValid)
            {
                c.Nombre = cliente.Nombre;
                c.DUI = cliente.DUI;
                c.Telefono = cliente.Telefono;

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // ----------- Eliminar --------------
        public IActionResult Delete(int id) // GET
        {
            var cliente = ClientesList.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id) // POST
        {
            var cliente = ClientesList.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                ClientesList.Remove(cliente);
            }
            return RedirectToAction("Index");
        }
    }
}
