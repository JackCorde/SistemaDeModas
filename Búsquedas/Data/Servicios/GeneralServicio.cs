using System.Data;
using Búsquedas.Models;
using Microsoft.Data.SqlClient;

namespace Búsquedas.Data.Servicios
{
    public class GeneralServicio
    {
        private readonly Contexto _contexto;

        public GeneralServicio(Contexto contexto)
        {
            _contexto = contexto;
        }

        public List<Producto> ListaProductoSimple(string busqueda )
        {
            
            var model = new List<Producto>();
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (SqlCommand cmd = new("busquedaSimple", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Palabra", busqueda);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        model.Add(new Producto
                        {
                            IdProducto = (int)reader["idProducto"],
                            Modelo = (String)reader["modelo"],
                            Tipo = (String)reader["tipo"],
                            Talla = (String)reader["talla"],
                            Categoria = (String)reader["categoria"],
                            Marca = (String)reader["marca"]
                        });

                    }
                }
            }
            return model;
        }

        public List<Producto> ListaProductoComplejo(string? busqueda, int? tipo, int? talla, int? categoria, int? marca)
        {
            var model = new List<Producto>();
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (SqlCommand cmd = new("busquedaCompleja", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Palabra", busqueda);
                    cmd.Parameters.AddWithValue("@Tipo", tipo);
                    cmd.Parameters.AddWithValue("@Talla", talla);
                    cmd.Parameters.AddWithValue("@Categoria", categoria);
                    cmd.Parameters.AddWithValue("@Marca", marca);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        model.Add(new Producto
                        {
                            IdProducto = (int)reader["idProducto"],
                            Modelo = (String)reader["modelo"],
                            Tipo = (String)reader["tipo"],
                            Talla = (String)reader["talla"],
                            Categoria = (String)reader["categoria"],
                            Marca = (String)reader["marca"]
                        });

                    }
                }
            }
            return model;
        }

        public Producto ConsultarProducto(int id)
        {
            Producto model;
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (SqlCommand cmd = new("consultaProducto", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read()) {
                        model = new Producto
                        {
                            IdProducto = (int)reader["idProducto"],
                            Modelo = (String)reader["modelo"],
                            Tipo = (String)reader["tipo"],
                            Talla = (String)reader["talla"],
                            Categoria = (String)reader["categoria"],
                            Marca = (String)reader["marca"]
                        };
                    }
                    else
                    {
                        model = new Producto
                        {
                            IdProducto = null,
                            Modelo = null,
                            Tipo = null,
                            Talla = null,
                            Categoria = null,
                            Marca = null
                        };
                    }
                }
            }
            return model;
        }

        public List<Comentario> ConsultarComentarios(int id)
        {
            var model = new List<Comentario>();
            using (var connection = new SqlConnection(_contexto.Conexion))
            {
                connection.Open();
                using (SqlCommand cmd = new("consultarComentarios", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        model.Add(new Comentario
                        {
                            IdComentario = (int)reader["idComentario"],
                            ProductoId = (int)reader["productoId"],
                            UsuarioNombre = (String)reader["usuarioNombre"],
                            UsuarioId = (int)reader["usuarioId"],
                            Fecha = (DateTime)reader["fecha"],
                            Contenido = (String)reader["comentario"]
                        });

                    }
                }
            }
            return model;
        }

    }
}
