namespace Sunpincol.Models;

public class Pedido
{
    public int IdPedido { get; set; }
    public int IdSedeSolicitante { get; set; }
    public int IdEmpleadoSolicitante { get; set; }
    public string CodigoProducto { get; set; } = null!;
    public int CantidadSolicitada { get; set; }
    public string EstadoPedido { get; set; } = null!;
    public DateTime FechaSolicitud { get; set; }
}