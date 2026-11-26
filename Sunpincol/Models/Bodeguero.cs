
namespace Sunpincol.Models;

public class Bodeguero
{
    public int IdBodeguero { get; set; }
    public int IdEmpleado { get; set; }
    public int IdBodegaAsignada { get; set; }
    public string ZonaAlmacenamiento { get; set; } = null!;
    public string Turno { get; set; } = null!;
}
