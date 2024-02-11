using Búsquedas.Data.Servicios;
using Búsquedas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Búsquedas.Models.ViewModels;
using Búsquedas.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public ActionResult Detalles(int id)
        {
            Producto  producto = _generalServicio.ConsultarProducto(id);
            ViewBag.Producto = producto;
            var Comentarios = _generalServicio.ConsultarComentarios(id);
            ViewBag.Comentarios = Comentarios;
            return View();
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Comentario(Comentario model)
        {
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var command = new SqlCommand("agregarComentario", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Comentario", model.Contenido);
                        command.Parameters.AddWithValue("@Producto", model.ProductoId);
                        command.Parameters.AddWithValue("@Usuario", model.UsuarioId);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Detalles", "Usuario", new { id = model.ProductoId, Error = "No se pudo registrar el nuevo comentario" });
                    }
                }
            }
            return RedirectToAction("Detalles", "Usuario", new {id=model.ProductoId});
        }

    }
}
