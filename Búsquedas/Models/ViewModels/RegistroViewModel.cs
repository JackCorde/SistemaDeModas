using Búsquedas.Data;

namespace Búsquedas.Models.ViewModels
{
    public class RegistroViewModel
    {
        public string? Nombre { get; set; } = null!;
        public string? ApellidoPaterno { get; set; } = null!;
        public string? ApellidoMaterno { get; set; } = null!;
        public string? Username { get; set; }
        public string? Pass { get; set; }
        public string? PassConf { get; set; }
        public string? Correo { get; set; }
        public string? Error { get; set; }
    }
}
