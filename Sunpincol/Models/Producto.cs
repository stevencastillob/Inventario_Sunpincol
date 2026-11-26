
namespace Sunpincol.Models;

public class Producto
{
    public string CodigoProducto { get; set; } = null!;

    public string NombreProducto { get; set; } = null!;

    public string Categoria { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal PrecioUnitario { get; set; }

    public bool EstadoProducto { get; set; }
}
