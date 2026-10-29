namespace ControlEscolar.Models;

public class Alumno
{
    public int Matricula { get; set; }
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";
    public string Telefono { get; set; } = "";
    public int IdCarrera { get; set; }
}
