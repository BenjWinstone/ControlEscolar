using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia;

namespace ControlEscolar.Services;

/// <summary>Reemplazo de MessageBox.Show (Avalonia no trae uno).</summary>
public static class Dialogos
{
    public static Task MostrarAsync(Window propietario, string mensaje, string titulo = "Control Escolar") =>
        CrearAsync(propietario, titulo, mensaje, conCancelar: false);

    /// <summary>OK/Cancelar. Cancelar es el boton por defecto, igual que MessageBoxDefaultButton.Button2.</summary>
    public static Task<bool> ConfirmarAsync(Window propietario, string mensaje, string titulo = "Confirmar") =>
        CrearAsync(propietario, titulo, mensaje, conCancelar: true);

    public static async Task AvisarYEnfocarAsync(Window propietario, string mensaje, Control campo)
    {
        await MostrarAsync(propietario, mensaje);
        campo.Focus();
    }

    /// <summary>Ejecuta una accion y muestra cualquier error en un dialogo en lugar de cerrar la app.</summary>
    public static async Task EjecutarAsync(Window propietario, Func<Task> accion)
    {
        try
        {
            await accion();
        }
        catch (Exception ex)
        {
            await MostrarAsync(propietario, Errores.Mensaje(ex), "Error");
        }
    }

    private static async Task<bool> CrearAsync(Window propietario, string titulo, string mensaje, bool conCancelar)
    {
        var aceptar = new Button { Content = "Aceptar", MinWidth = 90, IsDefault = !conCancelar };
        var cancelar = new Button { Content = "Cancelar", MinWidth = 90, IsDefault = conCancelar, IsCancel = true };

        var botones = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        botones.Children.Add(aceptar);
        if (conCancelar)
            botones.Children.Add(cancelar);

        var contenido = new StackPanel { Margin = new Thickness(20), Spacing = 16, MaxWidth = 460 };
        contenido.Children.Add(new TextBlock { Text = mensaje, TextWrapping = TextWrapping.Wrap });
        contenido.Children.Add(botones);

        var ventana = new Window
        {
            Title = titulo,
            Content = contenido,
            SizeToContent = SizeToContent.WidthAndHeight,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        aceptar.Click += (_, _) => ventana.Close(true);
        cancelar.Click += (_, _) => ventana.Close(false);

        // Si se cierra con la X, el resultado es false.
        return await ventana.ShowDialog<bool>(propietario);
    }
}
