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
    }
}
