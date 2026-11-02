using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace projMenuGeneral
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Igual que Application.Run(new frmMenuGeneral()): al cerrar la ventana principal se cierra el programa
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                // CAMBIO: ahora se arranca con el login; frmLogin cambia MainWindow al menu cuando el acceso es correcto
                desktop.MainWindow = new frmLogin();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
