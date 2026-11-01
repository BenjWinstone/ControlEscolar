using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using projMenuGeneral.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace projMenuGeneral
{
    public partial class frmLogin : Window
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private async Task<bool> CampoVacio(TextBox campo, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(campo.Text))
            {
                await MessageBox.Show("Escriba " + descripcion);
                campo.Focus();
                return true;
            }
            return false;
        }
        Clases.clsConexion c;
        private async Task Entrar()
        {
            if (await CampoVacio(txtUsuario, "el usuario") || await CampoVacio(txtContrasena, "la contrasena"))
                return;

            DataSet ds = new DataSet();
            clsUsuarios usuario = new clsUsuarios(txtUsuario.Text, txtContrasena.Text);
            c = new Clases.clsConexion(usuario.Login());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count == 0)
            {
                await MessageBox.Show("Usuario o contrasena incorrectos");
                txtContrasena.Text = "";
                txtContrasena.Focus();
                return;
            }

            DataRow row = ds.Tables["Tabla"].Rows[0];
            Clases.clsGlobal._idUsuario = Convert.ToInt32(row["iIdUsuario"]);
            Clases.clsGlobal._usuario = row["vUsuario"].ToString();
            Clases.clsGlobal._rol = Convert.ToByte(row["iRol"]);

            // IMPORTANTE: primero se cambia MainWindow al menu y despues se cierra el login.
            // Si se cerrara primero, ShutdownMode.OnMainWindowClose apagaria todo el programa.
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                frmMenuGeneral menu = new frmMenuGeneral();
                desktop.MainWindow = menu;
                menu.Show();
                Close();
            }
        }

        private async void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            await Entrar();
        }
    }
}
