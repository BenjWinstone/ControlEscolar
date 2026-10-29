namespace ControlEscolar.Models;

public class Maestro
{
    public int NumeroEmpleado { get; set; }
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";
    public string Telefono { get; set; } = "";
    public int IdCarrera { get; set; }
}
