using Avalonia.Controls;
using Avalonia.Interactivity;
using projMenuGeneral.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace projMenuGeneral.Escolar
{
    public partial class frmCarreras : Window
    {
        public frmCarreras()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)   // CAMBIO: la vista previa de Rider no debe conectarse a la base de datos
                CargarDepartamentos();
        }
        private void CargarDepartamentos()
        {
            DataSet ds = new DataSet();
            Clases.clsDepartamentos s = new Clases.clsDepartamentos();
            Clases.clsConexion c = new Clases.clsConexion(s.ConsultaGeneral());
            ds = c.Consultar();
            LlenarComboDepartamentos(ds.Tables[0]);   // CAMBIO: antes DataSource / DisplayMember / ValueMember
        }
        private List<clsOpcion> _departamentos = new List<clsOpcion>();
        private void LlenarComboDepartamentos(DataTable dt)
        {
            _departamentos.Clear();
            foreach (DataRow fila in dt.Rows)
                _departamentos.Add(new clsOpcion(Convert.ToInt32(fila["iIdDepartamento"]), fila["vDepartamento"].ToString()));
            comboDepartamento.ItemsSource = _departamentos;
        }
        private int ValorDepartamento()
        {
            return ((clsOpcion)comboDepartamento.SelectedItem).Valor;
        }
        private void SeleccionarDepartamento(int idDepartamento)
        {
            comboDepartamento.SelectedItem = _departamentos.FirstOrDefault(o => o.Valor == idDepartamento);
        }
        private async Task<bool> ObtenerEntero(TextBox campo, string descripcion)
        {
            if (!int.TryParse(campo.Text, out int valor) || valor <= 0)
            {
                await MessageBox.Show(descripcion + " debe de ser un numero entero positivo");
                campo.Focus();
                return false;
            }
            return true;
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
        private async Task Buscar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsCarreras carrera = new clsCarreras(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(carrera.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                txtNombre.Text = ds.Tables["Tabla"].Rows[0]["vCarrera"].ToString();
                SeleccionarDepartamento(Convert.ToInt32(ds.Tables["Tabla"].Rows[0]["iIdDepartamento"]));
            }
            else
                await MessageBox.Show("No existe esa Carrera");
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtClave, "La clave") || await CampoVacio(txtNombre, "el nombre de la carrera"))
                return;
            if (comboDepartamento.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione un departamento");
                comboDepartamento.Focus();
                return;
            }
            byte idDepartamento = (byte)ValorDepartamento();
            
            DataSet ds = new DataSet();
            clsCarreras carrera = new clsCarreras(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(carrera.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                carrera = new clsCarreras(int.Parse(txtClave.Text), txtNombre.Text, idDepartamento);
                c = new Clases.clsConexion(carrera.Modificar());
                c.Ejecutar();
                await MessageBox.Show("Registro Existente Modificado");
            }
            else
            {
                carrera = new clsCarreras(int.Parse(txtClave.Text), txtNombre.Text, idDepartamento);
                c = new Clases.clsConexion(carrera.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsCarreras carrera = new clsCarreras(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(carrera.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdCarrera"] + Environment.NewLine +
                    "Nombre: " + row["vCarrera"];

                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.OK)
                {
                    c = new Clases.clsConexion(carrera.Eliminar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Eliminado");
                }
            }
            else
            {
                await MessageBox.Show("No existe esa Carrera");
            }

        }

        private async void msRegistrarCarrera_Click(object sender, RoutedEventArgs e)
        {
            await Guardar();
        }
        private async void tsRegistrarCarrera_Click(object sender, RoutedEventArgs e)
        {
            await Guardar();
        }

        private async void msBuscarCarrera_Click(object sender, RoutedEventArgs e)
        {
            await Buscar();
        }
        private async void tsBuscarCarrera_Click(object sender, RoutedEventArgs e)
        {
            await Buscar();
        }

        private async void msEliminarCarrera_Click(object sender, RoutedEventArgs e)
        {
            await Eliminar();
        }
        private async void tsEliminarCarrera_Click(object sender, RoutedEventArgs e)
        {
            await Eliminar();
        }

        private void msSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
