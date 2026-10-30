using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Data.SqlClient;
using projMenuGeneral.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace projMenuGeneral.Escolar
{
    public partial class frmAlumnos : Window
    {
        public frmAlumnos()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)   // CAMBIO: la vista previa de Rider no debe conectarse a la base de datos
                CargarCarreras();
        }
        private void CargarCarreras()
        {
            DataSet ds = new DataSet();
            Clases.clsCarreras s = new Clases.clsCarreras();
            Clases.clsConexion c = new Clases.clsConexion(s.ConsultaGeneral());
            ds = c.Consultar();
            LlenarComboCarreras(ds.Tables[0]);
        }
        // CAMBIO (Avalonia): el ComboBox de WinForms usaba DataSource/DisplayMember/ValueMember.
        // Aqui se llena con objetos clsOpcion (Valor = iIdCarrera, Texto = vCarrera) y estos 3 metodos
        // hacen el trabajo de SelectedValue.
        private List<clsOpcion> _carreras = new List<clsOpcion>();
        private void LlenarComboCarreras(DataTable dt)
        {
            _carreras.Clear();
            foreach (DataRow fila in dt.Rows)
                _carreras.Add(new clsOpcion(Convert.ToInt32(fila["iIdCarrera"]), fila["vCarrera"].ToString()));
            comboCarreras.ItemsSource = _carreras;
        }
        private int ValorCarrera()
        {
            return ((clsOpcion)comboCarreras.SelectedItem).Valor;
        }
        private void SeleccionarCarrera(int idCarrera)
        {
            comboCarreras.SelectedItem = _carreras.FirstOrDefault(o => o.Valor == idCarrera);
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
        private bool CorreoValido(string correo)
        {
            if (correo.Contains(" "))
                return false;
            int arroba = correo.IndexOf('@');
            if (arroba <= 0)
                return false;
            if (arroba != correo.LastIndexOf('@'))
                return false;

            string dominio = correo.Substring(arroba + 1);
            return dominio.Contains(".") && !dominio.StartsWith(".") && !dominio.EndsWith(".");
        }
        private async Task<bool> ValidarAlumno()
        {
            if (await CampoVacio(txtNombre, "el nombre del alumno")) return false;
            if (await CampoVacio(txtCorreo, "el correo")) return false;
            if (!CorreoValido(txtCorreo.Text))
            {
                await MessageBox.Show("El correo no es valido");
                txtCorreo.Focus();
                return false;
            }
            if (await CampoVacio(txtTelefono, "el telefono")) return false;
            if (txtTelefono.Text.Length != 10 || !txtTelefono.Text.All(char.IsDigit))
            {
                await MessageBox.Show("El telefono debe de tener 10 digitos");
                txtTelefono.Focus();
                return false;
            }
            if (comboCarreras.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione una carrera");
                comboCarreras.Focus();
                return false;
            }
            return true;
        }
        Clases.clsConexion c;
        private async Task Buscar()
        {
            if (!await ObtenerEntero(txtMatricula, "La matricula"))
                return;

            DataSet ds = new DataSet();
            clsAlumnos alumno = new clsAlumnos(int.Parse(txtMatricula.Text));
            c = new Clases.clsConexion(alumno.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                txtNombre.Text = ds.Tables["Tabla"].Rows[0]["vNombre"].ToString();
                txtTelefono.Text = ds.Tables["Tabla"].Rows[0]["vTelefono"].ToString();
                txtCorreo.Text = ds.Tables["Tabla"].Rows[0]["vCorreo"].ToString();
                SeleccionarCarrera(Convert.ToInt32(ds.Tables["Tabla"].Rows[0]["iIdCarrera"]));
            }
            else
                await MessageBox.Show("No existe esa Matricula");
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtMatricula, "La matricula") || !await ValidarAlumno())
                return;

            DataSet ds = new DataSet();
            clsAlumnos alumno = new clsAlumnos(int.Parse(txtMatricula.Text));
            c = new Clases.clsConexion(alumno.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                alumno = new clsAlumnos(int.Parse(txtMatricula.Text), txtNombre.Text, txtCorreo.Text, txtTelefono.Text, ValorCarrera());
                c = new Clases.clsConexion(alumno.Modificar());
                c.Ejecutar();
                await MessageBox.Show("Registro Existente Modificado");
            }
            else
            {
                alumno = new clsAlumnos(int.Parse(txtMatricula.Text), txtNombre.Text, txtCorreo.Text, txtTelefono.Text, ValorCarrera());
                c = new Clases.clsConexion(alumno.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtMatricula, "La matricula"))
                return;

            DataSet ds = new DataSet();
            clsAlumnos alumno = new clsAlumnos(int.Parse(txtMatricula.Text));
            c = new Clases.clsConexion(alumno.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Matricula: " + row["iMatricula"] + Environment.NewLine +
                    "Nombre: " + row["vNombre"] + Environment.NewLine +
                    "Correo: " + row["vCorreo"] + Environment.NewLine +
                    "Telefono: " + row["vTelefono"];
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.OK)
                {
                    c = new Clases.clsConexion(alumno.Eliminar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Eliminado");
                }
            } else
            {
                await MessageBox.Show("No existe esa Matricula");
            }

        }

        private async void msRegistrar_Click(object sender, RoutedEventArgs e)
        {
            await Guardar();
        }
        private async void tsRegistrar_Click(object sender, RoutedEventArgs e)
        {
            await Guardar();
        }

        private async void msBuscar_Click(object sender, RoutedEventArgs e)
        {
            await Buscar();
        }
        private async void tsBuscar_Click(object sender, RoutedEventArgs e)
        {
            await Buscar();
        }

        private async void msEliminar_Click(object sender, RoutedEventArgs e)
        {
            await Eliminar();
        }
        private async void tsEliminar_Click(object sender, RoutedEventArgs e)
        {
            await Eliminar();
        }

        private void msSalir_Click(object sender, RoutedEventArgs e)
        {
            Clases.clsGlobal.Salir();
        }
    }
}
