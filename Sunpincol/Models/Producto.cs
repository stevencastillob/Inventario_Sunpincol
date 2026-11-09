using System;
using System.Collections.Generic;

namespace Sunpincol.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Stock { get; set; }

    public string? Categoria { get; set; }

    public int? Precio { get; set; }
    public string? Descripcion { get; set; }
    public string? guia { get; set; }
}
