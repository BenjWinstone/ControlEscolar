using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ControlEscolar.Views;

/// <summary>
/// Menu "Opciones" + barra de botones, compartidos por todas las ventanas.
/// Sustituye al MenuStrip y ToolStrip que estaban duplicados en cada formulario.
/// </summary>
public partial class BarraAcciones : UserControl
{
    public event EventHandler? Registrar;
    public event EventHandler? Buscar;
    public event EventHandler? Eliminar;
    public event EventHandler? Salir;

    public BarraAcciones()
    {
        InitializeComponent();
    }

    private void Registrar_Click(object? sender, RoutedEventArgs e) => Registrar?.Invoke(this, EventArgs.Empty);
    private void Buscar_Click(object? sender, RoutedEventArgs e) => Buscar?.Invoke(this, EventArgs.Empty);
    private void Eliminar_Click(object? sender, RoutedEventArgs e) => Eliminar?.Invoke(this, EventArgs.Empty);
    private void Salir_Click(object? sender, RoutedEventArgs e) => Salir?.Invoke(this, EventArgs.Empty);
}
