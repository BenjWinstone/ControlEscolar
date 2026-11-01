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
    public partial class frmMunicipios : Window
    {
        public frmMunicipios()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)   // la vista previa de Rider no debe conectarse a la base de datos
                CargarEstados();
        }
        private void CargarEstados()
        {
            DataSet ds = new DataSet();
            Clases.clsEstados s = new Clases.clsEstados();
            Clases.clsConexion cn = new Clases.clsConexion(s.ConsultaGeneral());
            ds = cn.Consultar();
            LlenarComboEstados(ds.Tables[0]);
        }
        private List<clsOpcion> _estados = new List<clsOpcion>();
        private void LlenarComboEstados(DataTable dt)
        {
            _estados = new List<clsOpcion>();
            foreach (DataRow fila in dt.Rows)
                _estados.Add(new clsOpcion(Convert.ToInt32(fila["iIdEstado"]), fila["vEstado"].ToString()));
            comboEstados.ItemsSource = _estados;
        }
        private int ValorEstado()
        {
            return ((clsOpcion)comboEstados.SelectedItem).Valor;
        }
        private void SeleccionarEstado(int idEstado)
        {
            comboEstados.SelectedItem = _estados.FirstOrDefault(o => o.Valor == idEstado);
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
            clsMunicipios municipio = new clsMunicipios(int.Parse(txtClave.Text), 0);
            c = new Clases.clsConexion(municipio.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                txtNombre.Text = ds.Tables["Tabla"].Rows[0]["vMunicipio"].ToString();
                SeleccionarEstado(Convert.ToInt32(ds.Tables["Tabla"].Rows[0]["iIdEstado"]));
            }
            else
            {
                await MessageBox.Show("No existe ese municipio");
                txtNombre.Text = "";
            }
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtClave, "La clave") || await CampoVacio(txtNombre, "el nombre del municipio"))
                return;
            if (comboEstados.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione un estado");
                comboEstados.Focus();
                return;
            }

            DataSet ds = new DataSet();
            clsMunicipios municipio = new clsMunicipios(int.Parse(txtClave.Text), 0);
            c = new Clases.clsConexion(municipio.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere actualizar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                                 "Clave: " + row["iIdMunicipio"] + Environment.NewLine +
                                 "Nombre: " + row["vMunicipio"] + "  ->  " + txtNombre.Text + Environment.NewLine +
                                 "Estado: " + row["iIdEstado"] + "  ->  " + ValorEstado();
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    municipio = new clsMunicipios(int.Parse(txtClave.Text), ValorEstado(), txtNombre.Text);
                    c = new Clases.clsConexion(municipio.Modificar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Existente Modificado");
                }
            }
            else
            {
                municipio = new clsMunicipios(int.Parse(txtClave.Text), ValorEstado(), txtNombre.Text);
                c = new Clases.clsConexion(municipio.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsMunicipios municipio = new clsMunicipios(int.Parse(txtClave.Text), 0);
            c = new Clases.clsConexion(municipio.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdMunicipio"] + Environment.NewLine +
                    "Nombre: " + row["vMunicipio"];
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    try
                    {
                        c = new Clases.clsConexion(municipio.Eliminar());
                        c.Ejecutar();
                        await MessageBox.Show("Registro Eliminado");
                    }
                    catch (Exception)
                    {
                        // Sin este try/catch, un FK violado cierra el programa completo
                        await MessageBox.Show("No se pudo eliminar. Revise que no tenga ciudades registradas");
                    }
                }
            }
            else
            {
                await MessageBox.Show("No existe ese municipio");
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
            Close();
        }
    }
}
