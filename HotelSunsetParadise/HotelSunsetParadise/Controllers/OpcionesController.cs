using Microsoft.AspNetCore.Mvc;

namespace HotelSunsetParadise.Controllers
{
    public class OpcionesController : Controller
    {
        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("Usuario");
            if(usuario == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Usuario = usuario;
            return View();
        }
    }
}
