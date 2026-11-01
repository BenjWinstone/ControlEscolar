using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    // NUEVA (Avalonia): Avalonia no trae MessageBox. Esta clase se usa igual que la de WinForms,
    // solo que hay que escribir "await" delante:
    //      await MessageBox.Show("Hola");
    //      DialogResult r = await MessageBox.Show(texto, "Titulo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
    // (el "await" es obligatorio porque en Avalonia las ventanas emergentes no detienen el programa,
    //  hay que esperar a que se cierren)
    public enum MessageBoxButtons { OK, OKCancel }
    public enum MessageBoxIcon { None, Warning }
    public enum MessageBoxDefaultButton { Button1, Button2 }
    public enum DialogResult { None, OK, Cancel }

    internal static class MessageBox
    {
        public static Task<DialogResult> Show(string mensaje)
        {
            return Show(mensaje, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public static async Task<DialogResult> Show(string mensaje, string titulo, MessageBoxButtons botones, MessageBoxIcon icono, MessageBoxDefaultButton botonPorDefecto)
        {
            // Si cierran la ventana con la X cuenta como Cancelar (o como OK si solo hay un boton)
            DialogResult resultado = (botones == MessageBoxButtons.OKCancel) ? DialogResult.Cancel : DialogResult.OK;

            Button aceptar = new Button();
            aceptar.Content = "Aceptar";
            aceptar.MinWidth = 80;
            aceptar.HorizontalContentAlignment = HorizontalAlignment.Center;

            Button cancelar = new Button();
            cancelar.Content = "Cancelar";
            cancelar.MinWidth = 80;
            cancelar.HorizontalContentAlignment = HorizontalAlignment.Center;
            cancelar.IsCancel = true;   // Esc = Cancelar

            StackPanel panelBotones = new StackPanel();
            panelBotones.Orientation = Orientation.Horizontal;
            panelBotones.Spacing = 8;
            panelBotones.HorizontalAlignment = HorizontalAlignment.Right;
            panelBotones.Children.Add(aceptar);
            if (botones == MessageBoxButtons.OKCancel)
                panelBotones.Children.Add(cancelar);

            // Boton por defecto (el que responde al Enter)
            if (botones == MessageBoxButtons.OKCancel && botonPorDefecto == MessageBoxDefaultButton.Button2)
                cancelar.IsDefault = true;
            else
                aceptar.IsDefault = true;

            TextBlock texto = new TextBlock();
            texto.Text = mensaje;
            texto.TextWrapping = Avalonia.Media.TextWrapping.Wrap;

            StackPanel contenido = new StackPanel();
            contenido.Margin = new Avalonia.Thickness(20);
            contenido.Spacing = 16;
            contenido.MaxWidth = 460;
            contenido.Children.Add(texto);
            contenido.Children.Add(panelBotones);

            Window ventana = new Window();
            ventana.Title = string.IsNullOrEmpty(titulo) ? "Aviso" : titulo;
            ventana.Content = contenido;
            ventana.SizeToContent = SizeToContent.WidthAndHeight;
            ventana.CanResize = false;
            ventana.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            aceptar.Click += (s, e) => { resultado = DialogResult.OK; ventana.Close(); };
            cancelar.Click += (s, e) => { resultado = DialogResult.Cancel; ventana.Close(); };

            // La ventana que esta activa es la duena del mensaje (la que el usuario esta usando)
            Window propietario = null;
            if (Avalonia.Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                propietario = desktop.Windows.FirstOrDefault(w => w.IsActive) ?? desktop.MainWindow;

            await ventana.ShowDialog(propietario);
            return resultado;
        }
    }
}
