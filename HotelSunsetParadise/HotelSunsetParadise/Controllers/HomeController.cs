using Microsoft.AspNetCore.Mvc;

namespace HotelSunsetParadise.Controllers
{
    public class HomeController : Controller
    {
        // CUENTA DE INGRESO
        private const string user = "admin";
        private const string pass = "123";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (username == user && password == pass)
            {
                HttpContext.Session.SetString("Usuario", username);

                // REDIRECCION
                return RedirectToAction("Index", "Opciones");
            }
            ViewBag.Error = "Credenciales inválidas"; // ERROR DE INGRESO DE CREDENCIALES
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
