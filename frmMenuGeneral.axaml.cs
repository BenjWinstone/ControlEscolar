using Avalonia.Controls;
using Avalonia.Interactivity;
using projMenuGeneral.Escolar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projMenuGeneral
{
    public partial class frmMenuGeneral : Window
    {
        public frmMenuGeneral()
        {
            InitializeComponent();
        }

        private void msAlumnos_Click(object sender, RoutedEventArgs e)
        {
            frmAlumnos alumnos = new frmAlumnos();
            alumnos.Show();
        }

        private void msMaestros_Click(object sender, RoutedEventArgs e)
        {
            frmMaestros maestros = new frmMaestros();
            maestros.Show();
        }

        private void msMaterias_Click(object sender, RoutedEventArgs e)
        {
            frmMaterias materias = new frmMaterias();
            materias.Show();
        }

        private void msCarreras_Click(object sender, RoutedEventArgs e)
        {
            frmCarreras carreras = new frmCarreras();
            carreras.Show();
        }

        private void msSuma_Click(object sender, RoutedEventArgs e)
        {
            //Formulario Suma
        }

        private void msResta_Click(object sender, RoutedEventArgs e)
        {
            //Formulario Resta
        }

        private void msMultiplicacion_Click(object sender, RoutedEventArgs e)
        {
            //Formulario Multi
        }

        private void msDivision_Click(object sender, RoutedEventArgs e)
        {
            //Formulario Division
        }

        private void msSalir_Click(object sender, RoutedEventArgs e)
        {
            Clases.clsGlobal.Salir();
        }
    }
}
