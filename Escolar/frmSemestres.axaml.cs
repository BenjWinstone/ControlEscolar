using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using projMenuGeneral.Clases;

namespace projMenuGeneral.Escolar
{
    public partial class frmSemestres : Window
    {
        public frmSemestres()
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
        Clases.clsConexion c;
        private async Task Buscar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsSemestre semestre = new clsSemestre(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(semestre.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                txtNivel.Text = ds.Tables["Tabla"].Rows[0]["vNivel"].ToString();
            }
            else
            { await MessageBox.Show("No existe ese semestre");
                txtNivel.Text = "";
            }
        }

        private async Task Guardar()
        {
            if (!await ObtenerEntero(txtClave, "La clave") || await CampoVacio(txtNivel, "el nivel del semestre"))
                return;

            DataSet ds = new DataSet();
            clsSemestre semestre = new clsSemestre(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(semestre.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                semestre = new clsSemestre(int.Parse(txtClave.Text), int.Parse(txtNivel.Text));
                c = new Clases.clsConexion(semestre.Modificar());
                c.Ejecutar();
                await MessageBox.Show("Registro Existente Modificado");
            }
            else
            {
                semestre = new clsSemestre(int.Parse(txtClave.Text), int.Parse(txtNivel.Text));
                c = new Clases.clsConexion(semestre.Guardar());
                c.Ejecutar();
                await MessageBox.Show("Registro Realizado con exito");
            }
        }

        private async Task Eliminar()
        {
            if (!await ObtenerEntero(txtClave, "La clave"))
                return;

            DataSet ds = new DataSet();
            clsSemestre semestre = new clsSemestre(int.Parse(txtClave.Text));
            c = new Clases.clsConexion(semestre.Buscar());
            ds = c.Consultar();
            if (ds.Tables["Tabla"].Rows.Count > 0)
            {
                DataRow row = ds.Tables["Tabla"].Rows[0];
                string mensaje = "¿Seguro que quiere eliminar el siguiente registro?" + Environment.NewLine + Environment.NewLine +
                    "Clave: " + row["iIdsemestre"] + Environment.NewLine +
                    "Nivel: " + row["vNivel"];

                DialogResult resultado = await MessageBox.Show(mensaje, "Confirmar eliminacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (resultado == DialogResult.OK)
                {
                    c = new Clases.clsConexion(semestre.Eliminar());
                    c.Ejecutar();
                    await MessageBox.Show("Registro Eliminado");
                }
            }
            else
            {
                await MessageBox.Show("No existe ese semestre");
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
