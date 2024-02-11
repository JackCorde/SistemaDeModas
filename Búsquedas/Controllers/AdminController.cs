using Búsquedas.Data;
using Búsquedas.Data.Servicios;
using Búsquedas.Models;
using Búsquedas.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Búsquedas.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _contexto;
        private readonly Data.Servicios.GeneralServicio _generalServicio;
        public AdminController(ILogger<HomeController> logger, Contexto contexto)
        {
             
            _logger = logger;
            _contexto = contexto;
            _generalServicio = new GeneralServicio(contexto);
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index(string Error)
        {
            var compras = _generalServicio.ListarComprasAdmin();
            ViewBag.Compras = compras;
            var pendientes = _generalServicio.ListarComprasPendientes();
            ViewBag.Pendientes = pendientes;
            ViewBag.Error = Error;
            return View();
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult Confirmar( int id)
        {
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var command = new SqlCommand("confirmarCompra", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Index", "Admin", new { Error = "No se pudo confirmar la venta" });
                    }
                }
            }
            return RedirectToAction("Index", "Admin");

        }

    }
}