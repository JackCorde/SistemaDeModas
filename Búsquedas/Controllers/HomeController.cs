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

        public IActionResult Index(BusquedaViewModel Busqueda, string Error)
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Usuario");
            }
            ViewBag.Error = Error;
            return View();
        }

        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User;
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Usuario");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Login");

            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    using (SqlCommand cmd = new("ConsultarUsuario", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", model.Username);
                        con.Open();
                        try
                        {
                            using (var dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    if (model.Pass != (string)dr["contrasena"])
                                    {
                                        ViewBag.Error = "Contraseña Incorrecta";
                                        dr.Close();
                                    }
                                    else
                                    {

                                        string? nombreusuario = (string)dr["username"];
                                        int idUsuario = (int)dr["idUsuario"];
                                        string? name = (string)dr["nombre"];
                                        string? ap = (string)dr["apellidoPaterno"];
                                        string? am = (string)dr["apellidoMaterno"];
                                        string nombreCompleto = name + " " + ap + " " + am;

                                        if (nombreusuario != null)
                                        {
                                            var claims = new List<Claim>()
                                                {
                                                    new Claim(ClaimTypes.NameIdentifier, nombreusuario),
                                                    new Claim(ClaimTypes.Name, nombreCompleto),
                                                    new Claim(ClaimTypes.SerialNumber, idUsuario.ToString())
                                                };

                                            int perfilId = (int)dr["tipoUsuarioId"];
                                            string perfilNombre = perfilId == 2 ? "Administrador" : "Usuario";
                                            claims.Add(new Claim(ClaimTypes.Role, perfilNombre));

                                            var identify = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                            var propiedades = new AuthenticationProperties
                                            {
                                                AllowRefresh = true,
                                                IsPersistent = true,
                                                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1)),
                                            };

                                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identify), propiedades);

                                            return RedirectToAction("Index", "Usuario");
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.Error = "Usuario no Registrado";
                                    dr.Close();
                                }
                            }
                        }
                        finally
                        {
                            cmd?.Dispose();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View("Login");
        }


        public IActionResult Registro(string Error)
        {
            ViewBag.Error = Error;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (model.Pass == model.PassConf)
            {
                try
                {
                    using (var connection = new SqlConnection(_contexto.Conexion))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("agregarUsuario", connection))
                        {
                            try
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@Nombre", model.Nombre);
                                command.Parameters.AddWithValue("@ApellidoPaterno", model.ApellidoPaterno);
                                command.Parameters.AddWithValue("@ApellidoMaterno", model.ApellidoMaterno);
                                command.Parameters.AddWithValue("@Username", model.Username);
                                command.Parameters.AddWithValue("@Correo", model.Correo);
                                command.Parameters.AddWithValue("@Contrasena", model.Pass);
                                command.ExecuteNonQuery();
                            }
                            catch (Exception)
                            {
                                return RedirectToAction("Registro", "Home", new { Error = "No se pudo registrar el nuevo cliente" });
                            }
                        }
                    }
                    return RedirectToAction("Registro");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627)
                    {
                        return RedirectToAction("Registro", "Home", new { Error = "El correo electronico ya se encuentra registrado." });
                    }
                    else
                        return RedirectToAction("Registro", "Home", new { Error = "Ocurrio un error al intentar registrar un cliente." + ex.Message });

                    throw;
                }
            }
            else
            {
                return RedirectToAction("Registro", "Home", new { Error = "Las contraseñas no son iguales." });
            }
            

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

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}