using Búsquedas.Data;
using Búsquedas.Data.Servicios;
using Búsquedas.Models;
using Búsquedas.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Búsquedas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _contexto;
        private readonly Data.Servicios.GeneralServicio _generalServicio;
        public HomeController(ILogger<HomeController> logger, Contexto contexto)
        {
             
            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }

        public IActionResult Index(BusquedaViewModel Busqueda)
        {
            return View();
        }

        public IActionResult Login(BusquedaViewModel Busqueda)
        {
            return View();
        }
        public IActionResult Registro(BusquedaViewModel Busqueda)
        {
            return View();
        }

        public IActionResult Recuperar(BusquedaViewModel Busqueda)
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}