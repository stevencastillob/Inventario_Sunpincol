
namespace Sunpincol.Models;

public class Historial
{
    public int IdHistorial { get; set; }
    public int IdPedido { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public string EstadoAnterior { get; set; } = null!;
    public string EstadoActual { get; set; } = null!;
    public DateTime FechaRegistro { get; set; }
}
