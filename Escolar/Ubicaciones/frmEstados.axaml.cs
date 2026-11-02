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
    public partial class frmEstados : Window
    {
        public frmEstados()
        {
            InitializeComponent();
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
        private async Task<bool> ClaveValida()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return false;
            return true;
        }
        Clases.clsConexion c;
        private async Task Buscar()
        {
            if (!await ClaveValida())
                return;

            DataSet ds = new DataSet();
            clsEstados estado = new clsEstados(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(estado.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                txtNombre.Text = ds.Tables["Tabla"].Rows[0]["vEstado"].ToString();
            }
            else
            {
                await MessageBox.Show("No existe ese estado");
                txtNombre.Text = "";
            }
        }

        private async Task Guardar()
        {
            if (!await ClaveValida() || await CampoVacio(txtNombre, "el nombre del estado"))
                return;

            DataSet ds = new DataSet();
            clsEstados estado = new clsEstados(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(estado.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere actualizar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                                 "Clave: " + row["iIdEstado"] + Environment.NewLine +
                                 "Nombre: " + row["vEstado"] + "  ->  " + txtNombre.Text;
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar modificacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    estado = new clsEstados(int.Parse(txtClave.Text), txtNombre.Text);
                    c = new Clases.clsConexion(estado.Modificar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Existente Modificado");
                }
            }
            else
            {
                estado = new clsEstados(int.Parse(txtClave.Text), txtNombre.Text);
                c = new Clases.clsConexion(estado.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ClaveValida())
                return;

            DataSet ds = new DataSet();
            clsEstados estado = new clsEstados(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(estado.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdEstado"] + Environment.NewLine +
                    "Nombre: " + row["vEstado"];
                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (resultado == DialogResult.OK)
                {
                    try
                    {
                        c = new Clases.clsConexion(estado.Eliminar());
                        c.Ejecutar();
                        await MessageBox.Show("Registro Eliminado");
                    }
                    catch (Exception)
                    {
                        // Sin este try/catch, un FK violado cierra el programa completo
                        await MessageBox.Show("No se pudo eliminar. Revise que no tenga municipios registrados");
                    }
                }
            }
            else
            {
                await MessageBox.Show("No existe ese estado");
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
