namespace Sunpincol.Models;

public class GuiaDespacho
{
    public int IdGuiaDespacho { get; set; }
    public int IdPedido { get; set; }
    public int IdBodegueroEmisor { get; set; }
    public string NumeroGuia { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
    public string EstadoEntrega { get; set; } = null!;
}