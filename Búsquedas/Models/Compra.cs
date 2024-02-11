using System;
using System.Collections.Generic;

namespace Búsquedas.Models;

public partial class Compra
{
    public int? IdCompra { get; set; }
    public int? UsuarioId { get; set; }
    public string? UsuarioNombre { get; set; }
    public bool? StatusCompra { get; set; }
    public DateTime? Fecha { get; set; }
    public int? ProductoId { get; set; }

    public string? Modelo { get; set; } = null!;

    public string? Tipo { get; set; }

    public string? Talla { get; set; } = null!;

    public string? Categoria { get; set; } = null!;

    public string? Marca { get; set; } = null!;


}
