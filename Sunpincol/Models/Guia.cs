namespace Sunpincol.Models;

public class Guia
{
    public int IdGuia { get; set; }
    public int IdSedeOrigen { get; set; }
    public int IdSedeDestino { get; set; }
    public string ReferenciaGuia { get; set; } = null!;
    public DateTime FechaEmision { get; set; }
    public string EstadoGuia { get; set; } = null!;
}