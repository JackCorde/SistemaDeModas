using Búsquedas.Data;

namespace Búsquedas.Models.ViewModels
{
    public class BusquedaViewModel
    {
        public string? Palabra { get; set; } = null!;
        public int? Tipo { get; set; } = null!;
        public int? Talla { get; set; } = null!;
        public int? Marca { get; set; } = null!;
        public int? Categoria { get; set; } = null!;
        public string? Username { get; set; }
        public string? Pass { get; set; }

    }
}
