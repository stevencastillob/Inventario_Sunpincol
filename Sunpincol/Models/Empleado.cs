
namespace Sunpincol.Models;

public class Empleado
{
    public int IdEmpleado { get; set; }
    public int IdSedeAsignada { get; set; }
    public string Cargo { get; set; } = null!;
    public string Turno { get; set; } = null!;
}
