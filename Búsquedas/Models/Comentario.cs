using System;
using System.Collections.Generic;

namespace Búsquedas.Models;

public partial class Comentario
{
    public int? IdComentario { get; set; }

    public int? ProductoId { get; set; } = null!;

    public string? UsuarioNombre { get; set; } = null!;
    public int? UsuarioId { get; set; }

    public DateTime? Fecha { get; set; } = null!;

    public string? Contenido { get; set; } = null!;
}
