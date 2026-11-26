namespace Sunpincol.Models;

public class Recepcion
{
    public int IdRecepcion { get; set; }
    public int IdPedido { get; set; }
    public int IdGuiaDespacho { get; set; }
    public int IdEmpleadoReceptor { get; set; }
    public int CantidadRecibida { get; set; }
    public bool Conformidad { get; set; }
    public DateTime FechaRecepcion { get; set; }
}