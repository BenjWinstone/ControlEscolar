using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ControlEscolar.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    // Show(this) hace que las ventanas hijas se cierren junto con el menu principal.
    private void Alumnos_Click(object? sender, RoutedEventArgs e) => new AlumnosWindow().Show(this);
    private void Maestros_Click(object? sender, RoutedEventArgs e) => new MaestrosWindow().Show(this);
    private void Materias_Click(object? sender, RoutedEventArgs e) => new MateriasWindow().Show(this);
    private void Carreras_Click(object? sender, RoutedEventArgs e) => new CarrerasWindow().Show(this);

    private void Salir_Click(object? sender, RoutedEventArgs e) => Close();
}
