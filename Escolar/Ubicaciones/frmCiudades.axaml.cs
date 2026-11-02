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
    public partial class frmCiudades : Window
    {
        public frmCiudades()
        {
            InitializeComponent();
            if (!Design.IsDesignMode)   // la vista previa de Rider no debe conectarse a la base de datos
                CargarEstados();
        }

        // ---------- Estados ----------
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
        private void SeleccionarEstado(int idEstado)
        {
            comboEstados.SelectedItem = _estados.FirstOrDefault(o => o.Valor == idEstado);
        }

        // ---------- Municipios (dependen del estado elegido) ----------
        private void CargarMunicipios(int idEstado)
        {
            DataSet ds = new DataSet();
            Clases.clsMunicipios s = new Clases.clsMunicipios(idEstado);
            Clases.clsConexion cn = new Clases.clsConexion(s.ConsultaPorEstado());
            ds = cn.Consultar();
            LlenarComboMunicipios(ds.Tables[0]);
        }
        private List<clsOpcion> _municipios = new List<clsOpcion>();
        private void LlenarComboMunicipios(DataTable dt)
        {
            _municipios = new List<clsOpcion>();
            foreach (DataRow fila in dt.Rows)
                _municipios.Add(new clsOpcion(Convert.ToInt32(fila["iIdMunicipio"]), fila["vMunicipio"].ToString()));
            comboMunicipios.ItemsSource = _municipios;
        }
        private int ValorMunicipio()
        {
            return ((clsOpcion)comboMunicipios.SelectedItem).Valor;
        }
        private void SeleccionarMunicipio(int idMunicipio)
        {
            comboMunicipios.SelectedItem = _municipios.FirstOrDefault(o => o.Valor == idMunicipio);
        }
        private void VaciarMunicipios()
        {
            _municipios = new List<clsOpcion>();
            comboMunicipios.ItemsSource = _municipios;
        }
        private void comboEstados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Design.IsDesignMode)
                return;
            if (comboEstados.SelectedItem == null)
                VaciarMunicipios();
            else
                CargarMunicipios(((clsOpcion)comboEstados.SelectedItem).Valor);
        }

        // ---------- Validaciones ----------
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

        // ---------- Buscar / Guardar / Eliminar ----------
        Clases.clsConexion c;
        private async Task Buscar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsCiudades ciudad = new clsCiudades(int.Parse(txtClave.Text), 0);
            c = new Clases.clsConexion(ciudad.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                txtNombre.Text = row["vCiudad"].ToString();
                // Seleccionar el estado carga los municipios, por eso van en este orden
                SeleccionarEstado(Convert.ToInt32(row["iIdEstado"]));
                SeleccionarMunicipio(Convert.ToInt32(row["iIdMunicipio"]));
            }
            else
            {
                await MessageBox.Show("No existe esa ciudad");
                txtNombre.Text = "";
            }
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtClave, "La clave") || await CampoVacio(txtNombre, "el nombre de la ciudad"))
                return;
            if (comboMunicipios.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione el estado y el municipio");
                comboEstados.Focus();
                return;
            }

            DataSet ds = new DataSet();
            clsCiudades ciudad = new clsCiudades(int.Parse(txtClave.Text), 0);
            c = new Clases.clsConexion(ciudad.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere actualizar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                                 "Clave: " + row["iIdCiudad"] + Environment.NewLine +
                                 "Nombre: " + row["vCiudad"] + "  ->  " + txtNombre.Text + Environment.NewLine +
                                 "Municipio: " + row["iIdMunicipio"] + "  ->  " + ValorMunicipio();
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    ciudad = new clsCiudades(int.Parse(txtClave.Text), ValorMunicipio(), txtNombre.Text);
                    c = new Clases.clsConexion(ciudad.Modificar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Existente Modificado");
                }
            }
            else
            {
                ciudad = new clsCiudades(int.Parse(txtClave.Text), ValorMunicipio(), txtNombre.Text);
                c = new Clases.clsConexion(ciudad.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsCiudades ciudad = new clsCiudades(int.Parse(txtClave.Text), 0);
            c = new Clases.clsConexion(ciudad.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdCiudad"] + Environment.NewLine +
                    "Nombre: " + row["vCiudad"];
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    try
                    {
                        c = new Clases.clsConexion(ciudad.Eliminar());
                        c.Ejecutar();
                        await MessageBox.Show("Registro Eliminado");
                    }
                    catch (Exception)
                    {
                        // Sin este try/catch, un FK violado cierra el programa completo
                        await MessageBox.Show("No se pudo eliminar. Revise que no tenga unidades registradas");
                    }
                }
            }
            else
            {
                await MessageBox.Show("No existe esa ciudad");
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
