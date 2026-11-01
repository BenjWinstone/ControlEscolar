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
            // Admin: ve todo + Ubicaciones + Solicitudes. Usuario: ve el boton para solicitar un lugar nuevo
            bool admin = Clases.clsGlobal.EsAdmin;
            msUbicaciones.IsVisible = admin;
            msSolicitudes.IsVisible = admin;
            btnSolicitar.IsVisible = !admin;
            if (!string.IsNullOrEmpty(Clases.clsGlobal._usuario))
                Title = "Menu General - " + Clases.clsGlobal._usuario;
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
        private void msDepartamentos_Click(object sender, RoutedEventArgs e)
        {
            frmDepartamentos departamentos = new frmDepartamentos();
            departamentos.Show();
        }
        private void msSemestres_Click(object sender, RoutedEventArgs e)
        {
            frmSemestres semestres = new frmSemestres();
            semestres.Show();
        }
        private void msTurnos_Click(object sender, RoutedEventArgs e)
        {
            frmTurnos turnos = new frmTurnos();
            turnos.Show();
        }
        private void msUnidades_Click(object sender, RoutedEventArgs e)
        {
            frmUnidades unidades = new frmUnidades();
            unidades.Show();
        }

        // ---------- Solo admin ----------
        private void msEstados_Click(object sender, RoutedEventArgs e)
        {
            frmEstados estados = new frmEstados();
            estados.Show();
        }
        private void msMunicipios_Click(object sender, RoutedEventArgs e)
        {
            frmMunicipios municipios = new frmMunicipios();
            municipios.Show();
        }
        private void msCiudades_Click(object sender, RoutedEventArgs e)
        {
            frmCiudades ciudades = new frmCiudades();
            ciudades.Show();
        }
        private void msSolicitudes_Click(object sender, RoutedEventArgs e)
        {
            frmSolicitudes solicitudes = new frmSolicitudes();
            solicitudes.Show();
        }

        // ---------- Solo usuario ----------
        private void btnSolicitar_Click(object sender, RoutedEventArgs e)
        {
            frmSolicitud solicitud = new frmSolicitud();
            solicitud.Show();
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
