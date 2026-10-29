using Avalonia.Controls;
using ControlEscolar.Data;
using ControlEscolar.Models;
using ControlEscolar.Services;

namespace ControlEscolar.Views;

public partial class AlumnosWindow : Window
{
    private readonly AlumnoRepository _alumnos = new();
    private readonly CarreraRepository _carreras = new();
    private List<Carrera> _listaCarreras = new();

    public AlumnosWindow()
    {
        InitializeComponent();

        // No se puede hacer await en el constructor: las carreras se cargan al abrir la ventana.
        Opened += async (_, _) => await Dialogos.EjecutarAsync(this, CargarCarrerasAsync);

        Barra.Registrar += async (_, _) => await GuardarAsync();
        Barra.Buscar += async (_, _) => await BuscarAsync();
        Barra.Eliminar += async (_, _) => await EliminarAsync();
        Barra.Salir += (_, _) => Close();
    }

    private async Task CargarCarrerasAsync()
    {
        _listaCarreras = await _carreras.ListarAsync();
        comboCarreras.ItemsSource = _listaCarreras;
    }

    private string NombreCarrera(int idCarrera) =>
        _listaCarreras.FirstOrDefault(c => c.IdCarrera == idCarrera)?.Nombre ?? idCarrera.ToString();

    private void LimpiarDatos()
    {
        txtNombre.Text = "";
        txtCorreo.Text = "";
        txtTelefono.Text = "";
        comboCarreras.SelectedItem = null;
    }

    private Task BuscarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtMatricula.Text, out int matricula))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "La matricula debe de ser un numero entero positivo", txtMatricula);
            return;
        }

        Alumno? alumno = await _alumnos.BuscarAsync(matricula);
        if (alumno == null)
        {
            LimpiarDatos();
            await Dialogos.MostrarAsync(this, "No existe esa Matricula");
            return;
        }

        txtNombre.Text = alumno.Nombre;
        txtCorreo.Text = alumno.Correo;
        txtTelefono.Text = alumno.Telefono;
        comboCarreras.SelectedItem = _listaCarreras.FirstOrDefault(c => c.IdCarrera == alumno.IdCarrera);
    });

    private Task GuardarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtMatricula.Text, out int matricula))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "La matricula debe de ser un numero entero positivo", txtMatricula);
            return;
        }

        string nombre = (txtNombre.Text ?? "").Trim();
        string correo = (txtCorreo.Text ?? "").Trim();
        string telefono = (txtTelefono.Text ?? "").Trim();

        if (nombre.Length == 0)
        {
            await Dialogos.AvisarYEnfocarAsync(this, "Escriba el nombre del alumno", txtNombre);
            return;
        }
        if (correo.Length == 0)
        {
            await Dialogos.AvisarYEnfocarAsync(this, "Escriba el correo", txtCorreo);
            return;
        }
        if (!Validaciones.EsCorreoValido(correo))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "El correo no es valido", txtCorreo);
            return;
        }
        if (telefono.Length == 0)
        {
            await Dialogos.AvisarYEnfocarAsync(this, "Escriba el telefono", txtTelefono);
            return;
        }
        if (!Validaciones.EsTelefonoValido(telefono))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "El telefono debe de tener 10 digitos", txtTelefono);
            return;
        }
        if (comboCarreras.SelectedItem is not Carrera carrera)
        {
            await Dialogos.AvisarYEnfocarAsync(this, "Seleccione una carrera", comboCarreras);
            return;
        }

        var nuevo = new Alumno
        {
            Matricula = matricula,
            Nombre = nombre,
            Correo = correo,
            Telefono = telefono,
            IdCarrera = carrera.IdCarrera
        };

        Alumno? existente = await _alumnos.BuscarAsync(matricula);
        if (existente == null)
        {
            await _alumnos.InsertarAsync(nuevo);
            await Dialogos.MostrarAsync(this, "Registro Realizado con exito");
            return;
        }

        string mensaje = "¿Seguro que quiere actualizar el siguiente registro?\n\n" +
                         $"Matricula: {existente.Matricula}\n" +
                         $"Nombre: {existente.Nombre}  ->  {nuevo.Nombre}\n" +
                         $"Correo: {existente.Correo}  ->  {nuevo.Correo}\n" +
                         $"Telefono: {existente.Telefono}  ->  {nuevo.Telefono}\n" +
                         $"Carrera: {NombreCarrera(existente.IdCarrera)}  ->  {carrera.Nombre}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar modificacion"))
            return;

        await _alumnos.ModificarAsync(nuevo);
        await Dialogos.MostrarAsync(this, "Registro Existente Modificado");
    });

    private Task EliminarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtMatricula.Text, out int matricula))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "La matricula debe de ser un numero entero positivo", txtMatricula);
            return;
        }

        Alumno? alumno = await _alumnos.BuscarAsync(matricula);
        if (alumno == null)
        {
            await Dialogos.MostrarAsync(this, "No existe esa Matricula");
            return;
        }

        string mensaje = "¿Seguro que quiere eliminar el siguiente registro?\n\n" +
                         $"Matricula: {alumno.Matricula}\n" +
                         $"Nombre: {alumno.Nombre}\n" +
                         $"Correo: {alumno.Correo}\n" +
                         $"Telefono: {alumno.Telefono}\n" +
                         $"Carrera: {NombreCarrera(alumno.IdCarrera)}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar eliminacion"))
            return;

        await _alumnos.EliminarAsync(matricula);
        await Dialogos.MostrarAsync(this, "Registro Eliminado");
        txtMatricula.Text = "";
        LimpiarDatos();
    });
}
