using Avalonia.Controls;
using Avalonia.Interactivity;
using projMenuGeneral.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace projMenuGeneral.Escolar
{
    public partial class frmSolicitud : Window
    {
        public frmSolicitud()
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
        private async Task Enviar()
        {
            if (await CampoVacio(txtTitulo, "el titulo de la solicitud") || await CampoVacio(txtDescripcion, "la descripcion de la solicitud"))
                return;

            clsSolicitudes solicitud = new clsSolicitudes(txtTitulo.Text, txtDescripcion.Text, Clases.clsGlobal._idUsuario);
            c = new Clases.clsConexion(solicitud.Guardar());
            c.Ejecutar();
            await MessageBox.Show("Solicitud enviada");
            Close();
        }

        private async void msEnviar_Click(object sender, RoutedEventArgs e)
        {
            await Enviar();
        }
        private async void tsEnviar_Click(object sender, RoutedEventArgs e)
        {
            await Enviar();
        }

        private void msSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
