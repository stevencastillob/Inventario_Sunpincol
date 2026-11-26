
namespace Sunpincol.Models;

public class Inventario
{
    public int IdInventario { get; set; }

    public int IdSede { get; set; }

    public string CodigoProducto { get; set; } = null!;

    public int StockActual { get; set; }

    public int StockMinimo { get; set; }

    public DateTime FechaUltimaActualizacion { get; set; }
}
