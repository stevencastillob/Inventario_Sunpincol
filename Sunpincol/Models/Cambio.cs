namespace Sunpincol.Models;

public class Cambio
{
    public int IdCambio { get; set; }
    public int IdInventario { get; set; }
    public int IdUsuarioResponsable { get; set; }
    public int CantidadAnterior { get; set; }
    public int CantidadNueva { get; set; }
    public string MotivoCambio { get; set; } = null!;
    public DateTime FechaCambio { get; set; }
}