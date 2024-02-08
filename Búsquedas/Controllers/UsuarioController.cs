using Búsquedas.Data.Servicios;
using Búsquedas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Búsquedas.Models.ViewModels;

namespace Búsquedas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly Contexto _contexto;
        private readonly Data.Servicios.GeneralServicio _generalServicio;

        public UsuarioController(ILogger<UsuarioController> logger, Contexto contexto)
        {
            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }

        // GET: UsuarioController
        public ActionResult Productos(BusquedaViewModel Busqueda)
        {
            int? tipo = Busqueda.Tipo;
            int? talla = Busqueda.Talla;
            int? categoria = Busqueda.Categoria;
            int? marca = Busqueda.Marca;
            string? busqueda = Busqueda.Palabra;
            var listado = _generalServicio.ListaProductoComplejo(busqueda, tipo, talla, categoria, marca);
            ViewBag.Listado = listado;
            return View();
        }


        public ActionResult Index()
        {
            return View();
        }



    }
}
