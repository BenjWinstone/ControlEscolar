namespace ControlEscolar.Models;

public class Carrera
{
    public int IdCarrera { get; set; }
    public string Nombre { get; set; } = "";

    // El ComboBox muestra el resultado de ToString()
    public override string ToString() => Nombre;
}
