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
    public partial class frmUnidades : Window
    {
        public frmUnidades()
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
            Clases.clsConexion c = new Clases.clsConexion(s.ConsultaGeneral());
            ds = c.Consultar();
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
            Clases.clsConexion c = new Clases.clsConexion(s.ConsultaPorEstado());
            ds = c.Consultar();
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
        private void SeleccionarMunicipio(int idMunicipio)
        {
            comboMunicipios.SelectedItem = _municipios.FirstOrDefault(o => o.Valor == idMunicipio);
        }

        // ---------- Ciudades (dependen del municipio elegido) ----------
        private void CargarCiudades(int idMunicipio)
        {
            DataSet ds = new DataSet();
            Clases.clsCiudades s = new Clases.clsCiudades(idMunicipio);
            Clases.clsConexion c = new Clases.clsConexion(s.ConsultaPorMunicipio());
            ds = c.Consultar();
            LlenarComboCiudades(ds.Tables[0]);
        }
        private List<clsOpcion> _ciudades = new List<clsOpcion>();
        private void LlenarComboCiudades(DataTable dt)
        {
            _ciudades = new List<clsOpcion>();
            foreach (DataRow fila in dt.Rows)
                _ciudades.Add(new clsOpcion(Convert.ToInt32(fila["iIdCiudad"]), fila["vCiudad"].ToString()));
            comboCiudades.ItemsSource = _ciudades;
        }
        private int ValorCiudad()
        {
            return ((clsOpcion)comboCiudades.SelectedItem).Valor;
        }
        private void SeleccionarCiudad(int idCiudad)
        {
            comboCiudades.SelectedItem = _ciudades.FirstOrDefault(o => o.Valor == idCiudad);
        }

        // ---------- Cascada: al cambiar un combo se recarga el siguiente ----------
        private void VaciarMunicipios()
        {
            _municipios = new List<clsOpcion>();
            comboMunicipios.ItemsSource = _municipios;
        }
        private void VaciarCiudades()
        {
            _ciudades = new List<clsOpcion>();
            comboCiudades.ItemsSource = _ciudades;
        }
        private void comboEstados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Design.IsDesignMode)
                return;
            VaciarCiudades();
            if (comboEstados.SelectedItem == null)
                VaciarMunicipios();
            else
                CargarMunicipios(((clsOpcion)comboEstados.SelectedItem).Valor);
        }
        private void comboMunicipios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Design.IsDesignMode)
                return;
            if (comboMunicipios.SelectedItem == null)
                VaciarCiudades();
            else
                CargarCiudades(((clsOpcion)comboMunicipios.SelectedItem).Valor);
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
        private async Task<bool> ValidarUnidad()
        {
            if (await CampoVacio(txtNombre, "el nombre de la unidad")) return false;
            if (txtNombre.Text.Length > 50)
            {
                await MessageBox.Show("El nombre de la unidad no puede tener mas de 50 caracteres");
                txtNombre.Focus();
                return false;
            }
            if (comboCiudades.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione el estado, el municipio y la ciudad");
                comboEstados.Focus();
                return false;
            }
            return true;
        }

        // ---------- Buscar / Guardar / Eliminar ----------
        Clases.clsConexion c;
        private async Task Buscar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsUnidades unidad = new clsUnidades(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(unidad.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                txtNombre.Text = row["vNombre"].ToString();
                // Cada Seleccionar... dispara la carga del siguiente combo, por eso van en este orden
                SeleccionarEstado(Convert.ToInt32(row["iIdEstado"]));
                SeleccionarMunicipio(Convert.ToInt32(row["iIdMunicipio"]));
                SeleccionarCiudad(Convert.ToInt32(row["iIdCiudad"]));
            }
            else
            {
                await MessageBox.Show("No existe esa unidad");
                txtNombre.Text = "";
            }
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtClave, "La clave") || !await ValidarUnidad())
                return;

            DataSet ds = new DataSet();
            clsUnidades unidad = new clsUnidades(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(unidad.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere actualizar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdUnidades"] + Environment.NewLine +
                    "Nombre: " + row["vNombre"] + "  ->  " + txtNombre.Text + Environment.NewLine +
                    "Ciudad: " + row["vCiudad"] + "  ->  " + comboCiudades.SelectedItem;
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    unidad = new clsUnidades(int.Parse(txtClave.Text), txtNombre.Text, ValorCiudad());
                    c = new Clases.clsConexion(unidad.Modificar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Existente Modificado");
                }
            }
            else
            {
                unidad = new clsUnidades(int.Parse(txtClave.Text), txtNombre.Text, ValorCiudad());
                c = new Clases.clsConexion(unidad.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsUnidades unidad = new clsUnidades(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(unidad.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdUnidades"] + Environment.NewLine +
                    "Nombre: " + row["vNombre"] + Environment.NewLine +
                    "Ciudad: " + row["vCiudad"];
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.OK)
                {
                    c = new Clases.clsConexion(unidad.Eliminar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Eliminado");
                }
            }
            else
            {
                await MessageBox.Show("No existe esa unidad");
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