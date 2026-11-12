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
    public partial class frmAsignaciones : Window
    {
        public frmAsignaciones()
        {
            InitializeComponent();
            LlenarComboEstado();
            if (!Design.IsDesignMode)   // la vista previa de Rider no debe conectarse a la base de datos
            {
                CargarMaestros();
                CargarMaterias();
            }
        }

        // ---------- Maestros ----------
        private void CargarMaestros()
        {
            DataSet ds = new DataSet();
            Clases.clsMaestros s = new Clases.clsMaestros();
            Clases.clsConexion cn = new Clases.clsConexion(s.ConsultaGeneral());
            ds = cn.Consultar();
            LlenarComboMaestros(ds.Tables[0]);
        }
        private List<clsOpcion> _maestros = new List<clsOpcion>();
        private void LlenarComboMaestros(DataTable dt)
        {
            _maestros = new List<clsOpcion>();
            foreach (DataRow fila in dt.Rows)
                _maestros.Add(new clsOpcion(Convert.ToInt32(fila["iNumeroEmpleado"]), fila["vNombre"].ToString()));
            comboMaestros.ItemsSource = _maestros;
        }
        private int ValorMaestro()
        {
            return ((clsOpcion)comboMaestros.SelectedItem).Valor;
        }
        private void SeleccionarMaestro(int numeroEmpleado)
        {
            comboMaestros.SelectedItem = _maestros.FirstOrDefault(o => o.Valor == numeroEmpleado);
        }

        // ---------- Materias ----------
        private void CargarMaterias()
        {
            DataSet ds = new DataSet();
            Clases.clsMaterias s = new Clases.clsMaterias();
            Clases.clsConexion cn = new Clases.clsConexion(s.ConsultaGeneral());
            ds = cn.Consultar();
            LlenarComboMaterias(ds.Tables[0]);
        }
        private List<clsOpcion> _materias = new List<clsOpcion>();
        private void LlenarComboMaterias(DataTable dt)
        {
            _materias = new List<clsOpcion>();
            foreach (DataRow fila in dt.Rows)
                _materias.Add(new clsOpcion(Convert.ToInt32(fila["iIdMateria"]), fila["vMateria"].ToString()));
            comboMaterias.ItemsSource = _materias;
        }
        private int ValorMateria()
        {
            return ((clsOpcion)comboMaterias.SelectedItem).Valor;
        }
        private void SeleccionarMateria(int idMateria)
        {
            comboMaterias.SelectedItem = _materias.FirstOrDefault(o => o.Valor == idMateria);
        }

        // ---------- Estado (1 = Vigente, 0 = Terminada) ----------
        private List<clsOpcion> _estados = new List<clsOpcion>();
        private void LlenarComboEstado()
        {
            _estados = new List<clsOpcion>();
            _estados.Add(new clsOpcion(1, "Vigente"));
            _estados.Add(new clsOpcion(0, "Terminada"));
            comboEstado.ItemsSource = _estados;
        }
        private bool ValorEstado()
        {
            return ((clsOpcion)comboEstado.SelectedItem).Valor == 1;
        }
        private void SeleccionarEstado(bool vigente)
        {
            comboEstado.SelectedItem = _estados.FirstOrDefault(o => o.Valor == (vigente ? 1 : 0));
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
        private async Task<bool> ValidarAsignacion()
        {
            if (comboMaestros.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione un maestro");
                comboMaestros.Focus();
                return false;
            }
            if (comboMaterias.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione una materia");
                comboMaterias.Focus();
                return false;
            }
            if (await CampoVacio(txtGrupo, "el grupo")) return false;
            if (await CampoVacio(txtSalon, "el salon")) return false;
            if (dateInicio.SelectedDate == null)
            {
                await MessageBox.Show("Seleccione la fecha de inicio");
                dateInicio.Focus();
                return false;
            }
            if (dateFin.SelectedDate == null)
            {
                await MessageBox.Show("Seleccione la fecha de fin");
                dateFin.Focus();
                return false;
            }
            if (dateFin.SelectedDate.Value < dateInicio.SelectedDate.Value)
            {
                await MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio");
                dateFin.Focus();
                return false;
            }
            if (comboEstado.SelectedItem == null)
            {
                await MessageBox.Show("Seleccione el estado de la asignacion");
                comboEstado.Focus();
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
            clsAsignaciones asignacion = new clsAsignaciones(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(asignacion.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                SeleccionarMaestro(Convert.ToInt32(row["iNumeroEmpleado"]));
                SeleccionarMateria(Convert.ToInt32(row["iIdMateria"]));
                txtGrupo.Text = row["vGrupo"].ToString();
                txtSalon.Text = row["vSalon"].ToString();
                dateInicio.SelectedDate = Convert.ToDateTime(row["dFechaInicio"]);
                dateFin.SelectedDate = Convert.ToDateTime(row["dFechaFin"]);
                SeleccionarEstado(Convert.ToBoolean(row["bVigente"]));
            }
            else
            {
                await MessageBox.Show("No existe esa asignacion");
                txtGrupo.Text = "";
                txtSalon.Text = "";
            }
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtClave, "La clave") || !await ValidarAsignacion())
                return;

            DataSet ds = new DataSet();
            clsAsignaciones asignacion = new clsAsignaciones(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(asignacion.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere actualizar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                                 "Clave: " + row["iIdAsignacion"] + Environment.NewLine +
                                 "Maestro: " + row["vMaestro"] + "  ->  " + comboMaestros.SelectedItem + Environment.NewLine +
                                 "Materia: " + row["vMateria"] + "  ->  " + comboMaterias.SelectedItem + Environment.NewLine +
                                 "Grupo: " + row["vGrupo"] + "  ->  " + txtGrupo.Text.Trim() + Environment.NewLine +
                                 "Salon: " + row["vSalon"] + "  ->  " + txtSalon.Text.Trim() + Environment.NewLine +
                                 "Inicio: " + Convert.ToDateTime(row["dFechaInicio"]).ToString("dd/MM/yyyy") + "  ->  " + dateInicio.SelectedDate.Value.ToString("dd/MM/yyyy") + Environment.NewLine +
                                 "Fin: " + Convert.ToDateTime(row["dFechaFin"]).ToString("dd/MM/yyyy") + "  ->  " + dateFin.SelectedDate.Value.ToString("dd/MM/yyyy") + Environment.NewLine +
                                 "Estado: " + (Convert.ToBoolean(row["bVigente"]) ? "Vigente" : "Terminada") + "  ->  " + comboEstado.SelectedItem;
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    asignacion = new clsAsignaciones(int.Parse(txtClave.Text), ValorMaestro(), ValorMateria(), txtGrupo.Text.Trim(), txtSalon.Text.Trim(), dateInicio.SelectedDate.Value, dateFin.SelectedDate.Value, ValorEstado());
                    c = new Clases.clsConexion(asignacion.Modificar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Existente Modificado");
                }
            }
            else
            {
                asignacion = new clsAsignaciones(int.Parse(txtClave.Text), ValorMaestro(), ValorMateria(), txtGrupo.Text.Trim(), txtSalon.Text.Trim(), dateInicio.SelectedDate.Value, dateFin.SelectedDate.Value, ValorEstado());
                c = new Clases.clsConexion(asignacion.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsAsignaciones asignacion = new clsAsignaciones(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(asignacion.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdAsignacion"] + Environment.NewLine +
                    "Maestro: " + row["vMaestro"] + Environment.NewLine +
                    "Materia: " + row["vMateria"] + Environment.NewLine +
                    "Grupo: " + row["vGrupo"];
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.OK)
                {
                    c = new Clases.clsConexion(asignacion.Eliminar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Eliminado");
                }
            }
            else
            {
                await MessageBox.Show("No existe esa asignacion");
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