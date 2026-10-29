using Avalonia.Controls;
using ControlEscolar.Data;
using ControlEscolar.Models;
using ControlEscolar.Services;

namespace ControlEscolar.Views;

public partial class MaestrosWindow : Window
{
    private readonly MaestroRepository _maestros = new();
    private readonly CarreraRepository _carreras = new();
    private List<Carrera> _listaCarreras = new();

    public MaestrosWindow()
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
        if (!Validaciones.EsEnteroPositivo(txtNumero.Text, out int numeroEmpleado))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "El numero de empleado debe de ser un numero entero positivo", txtNumero);
            return;
        }

        Maestro? maestro = await _maestros.BuscarAsync(numeroEmpleado);
        if (maestro == null)
        {
            LimpiarDatos();
            await Dialogos.MostrarAsync(this, "No existe ese Numero de Empleado");
            return;
        }

        txtNombre.Text = maestro.Nombre;
        txtCorreo.Text = maestro.Correo;
        txtTelefono.Text = maestro.Telefono;
        comboCarreras.SelectedItem = _listaCarreras.FirstOrDefault(c => c.IdCarrera == maestro.IdCarrera);
    });

    private Task GuardarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtNumero.Text, out int numeroEmpleado))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "El numero de empleado debe de ser un numero entero positivo", txtNumero);
            return;
        }

        string nombre = (txtNombre.Text ?? "").Trim();
        string correo = (txtCorreo.Text ?? "").Trim();
        string telefono = (txtTelefono.Text ?? "").Trim();

        if (nombre.Length == 0)
        {
            await Dialogos.AvisarYEnfocarAsync(this, "Escriba el nombre del maestro", txtNombre);
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

        var nuevo = new Maestro
        {
            NumeroEmpleado = numeroEmpleado,
            Nombre = nombre,
            Correo = correo,
            Telefono = telefono,
            IdCarrera = carrera.IdCarrera
        };

        Maestro? existente = await _maestros.BuscarAsync(numeroEmpleado);
        if (existente == null)
        {
            await _maestros.InsertarAsync(nuevo);
            await Dialogos.MostrarAsync(this, "Registro Realizado con exito");
            return;
        }

        string mensaje = "¿Seguro que quiere actualizar el siguiente registro?\n\n" +
                         $"Numero de empleado: {existente.NumeroEmpleado}\n" +
                         $"Nombre: {existente.Nombre}  ->  {nuevo.Nombre}\n" +
                         $"Correo: {existente.Correo}  ->  {nuevo.Correo}\n" +
                         $"Telefono: {existente.Telefono}  ->  {nuevo.Telefono}\n" +
                         $"Carrera: {NombreCarrera(existente.IdCarrera)}  ->  {carrera.Nombre}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar modificacion"))
            return;

        await _maestros.ModificarAsync(nuevo);
        await Dialogos.MostrarAsync(this, "Registro Existente Modificado");
    });

    private Task EliminarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtNumero.Text, out int numeroEmpleado))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "El numero de empleado debe de ser un numero entero positivo", txtNumero);
            return;
        }

        Maestro? maestro = await _maestros.BuscarAsync(numeroEmpleado);
        if (maestro == null)
        {
            await Dialogos.MostrarAsync(this, "No existe ese Numero de Empleado");
            return;
        }

        string mensaje = "¿Seguro que quiere eliminar el siguiente registro?\n\n" +
                         $"Numero de empleado: {maestro.NumeroEmpleado}\n" +
                         $"Nombre: {maestro.Nombre}\n" +
                         $"Correo: {maestro.Correo}\n" +
                         $"Telefono: {maestro.Telefono}\n" +
                         $"Carrera: {NombreCarrera(maestro.IdCarrera)}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar eliminacion"))
            return;

        await _maestros.EliminarAsync(numeroEmpleado);
        await Dialogos.MostrarAsync(this, "Registro Eliminado");
        txtNumero.Text = "";
        LimpiarDatos();
    });
}
