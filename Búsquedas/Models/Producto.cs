using System;
using System.Collections.Generic;

namespace Búsquedas.Models;

public partial class Producto
{
    public int? IdProducto { get; set; }

    public string? Modelo { get; set; } = null!;

    public string? Tipo { get; set; }

    public string? Talla { get; set; } = null!;

    public string? Categoria { get; set; } = null!;

    public string? Marca { get; set; } = null!;
}
