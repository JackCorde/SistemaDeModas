using Búsquedas.Data.Servicios;
using Búsquedas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Búsquedas.Models.ViewModels;
using Búsquedas.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;

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


        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Compras(int id, string? Error)
        {
            var listado = _generalServicio.ListarCompras(id);
            ViewBag.Compras = listado;
            ViewBag.Error = Error;
            return View();
        }
        [Authorize]
        public ActionResult Comprar(int id, int user)
        {
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (var command = new SqlCommand("agregarCompra", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Producto", id);
                        command.Parameters.AddWithValue("@Usuario", user);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Compras", "Usuario", new { id = user, Error = "No se pudo registrar el nuevo comentario" });
                    }
                }
            }
            return RedirectToAction("Compras", "Usuario", new { id = user });
        }

        [Authorize]
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

        [Authorize]
        public ActionResult Detalles(int id)
        {
            Producto  producto = _generalServicio.ConsultarProducto(id);
            ViewBag.Producto = producto;
            var Comentarios = _generalServicio.ConsultarComentarios(id);
            ViewBag.Comentarios = Comentarios;
            return View();
        }



        [Authorize]
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
