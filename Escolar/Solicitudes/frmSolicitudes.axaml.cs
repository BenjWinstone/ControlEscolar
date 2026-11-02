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
    public partial class frmSolicitudes : Window
    {
        public frmSolicitudes()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)   // la vista previa de Rider no debe conectarse a la base de datos
                CargarSolicitudes();
        }
        private List<clsOpcion> _solicitudes = new List<clsOpcion>();
        private Dictionary<int, string> _descripciones = new Dictionary<int, string>();
        private void CargarSolicitudes()
        {
            DataSet ds = new DataSet();
            Clases.clsSolicitudes s = new Clases.clsSolicitudes();
            Clases.clsConexion cn = new Clases.clsConexion(s.ConsultaPendientes());
            ds = cn.Consultar();

            _solicitudes = new List<clsOpcion>();
            _descripciones = new Dictionary<int, string>();
            foreach (DataRow fila in ds.Tables[0].Rows)
            {
                int id = Convert.ToInt32(fila["iIdSolicitud"]);
                string fecha = Convert.ToDateTime(fila["dFecha"]).ToString("dd/MM/yyyy");
                _solicitudes.Add(new clsOpcion(id, fecha + " - " + fila["vTitulo"] + " (" + fila["vUsuario"] + ")"));
                _descripciones[id] = fila["vDescripcion"].ToString();
            }
            listaSolicitudes.ItemsSource = _solicitudes;
            txtDescripcion.Text = "";
        }
        private void listaSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            clsOpcion seleccion = listaSolicitudes.SelectedItem as clsOpcion;
            if (seleccion != null && _descripciones.ContainsKey(seleccion.Valor))
                txtDescripcion.Text = _descripciones[seleccion.Valor];
            else
                txtDescripcion.Text = "";
        }
        Clases.clsConexion c;
        private async Task Atender()
        {
            clsOpcion seleccion = listaSolicitudes.SelectedItem as clsOpcion;
            if (seleccion == null)
            {
                await MessageBox.Show("Seleccione una solicitud");
                return;
            }
            clsSolicitudes solicitud = new clsSolicitudes(seleccion.Valor);
            c = new Clases.clsConexion(solicitud.MarcarAtendida());
            c.Ejecutar();
            await MessageBox.Show("Solicitud marcada como atendida");
            CargarSolicitudes();
        }

        private void msActualizar_Click(object sender, RoutedEventArgs e)
        {
            CargarSolicitudes();
        }
        private void tsActualizar_Click(object sender, RoutedEventArgs e)
        {
            CargarSolicitudes();
        }

        private async void msAtender_Click(object sender, RoutedEventArgs e)
        {
            await Atender();
        }
        private async void tsAtender_Click(object sender, RoutedEventArgs e)
        {
            await Atender();
        }

        private void msSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
