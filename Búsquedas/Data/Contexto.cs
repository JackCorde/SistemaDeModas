

namespace Búsquedas.Data
{
    public class Contexto{
        public Contexto(String valor)
        {
            Conexion = valor;
            
        }

        public string Conexion { get; }
         
    }
}
