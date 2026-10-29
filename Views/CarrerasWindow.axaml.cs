using Avalonia.Controls;
using ControlEscolar.Data;
using ControlEscolar.Models;
using ControlEscolar.Services;

namespace ControlEscolar.Views;

public partial class CarrerasWindow : Window
{
    private readonly CarreraRepository _repo = new();

    public CarrerasWindow()
    {
        InitializeComponent();
        Barra.Registrar += async (_, _) => await GuardarAsync();
        Barra.Buscar += async (_, _) => await BuscarAsync();
        Barra.Eliminar += async (_, _) => await EliminarAsync();
        Barra.Salir += (_, _) => Close();
    }

    private Task BuscarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtClave.Text, out int id))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "La clave debe de ser un numero entero positivo", txtClave);
            return;
        }

        Carrera? carrera = await _repo.BuscarAsync(id);
        if (carrera == null)
        {
            txtNombre.Text = "";
            await Dialogos.MostrarAsync(this, "No existe esa Carrera");
            return;
        }
        txtNombre.Text = carrera.Nombre;
    });

    private Task GuardarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtClave.Text, out int id))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "La clave debe de ser un numero entero positivo", txtClave);
            return;
        }

        string nombre = (txtNombre.Text ?? "").Trim();
        if (nombre.Length == 0)
        {
            await Dialogos.AvisarYEnfocarAsync(this, "Escriba el nombre de la carrera", txtNombre);
            return;
        }

        var nueva = new Carrera { IdCarrera = id, Nombre = nombre };
        Carrera? existente = await _repo.BuscarAsync(id);

        if (existente == null)
        {
            await _repo.InsertarAsync(nueva);
            await Dialogos.MostrarAsync(this, "Registro Realizado con exito");
            return;
        }

        string mensaje = "¿Seguro que quiere actualizar el siguiente registro?\n\n" +
                         $"Clave: {existente.IdCarrera}\n" +
                         $"Nombre: {existente.Nombre}  ->  {nueva.Nombre}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar modificacion"))
            return;

        await _repo.ModificarAsync(nueva);
        await Dialogos.MostrarAsync(this, "Registro Existente Modificado");
    });

    private Task EliminarAsync() => Dialogos.EjecutarAsync(this, async () =>
    {
        if (!Validaciones.EsEnteroPositivo(txtClave.Text, out int id))
        {
            await Dialogos.AvisarYEnfocarAsync(this, "La clave debe de ser un numero entero positivo", txtClave);
            return;
        }

        Carrera? carrera = await _repo.BuscarAsync(id);
        if (carrera == null)
        {
            await Dialogos.MostrarAsync(this, "No existe esa Carrera");
            return;
        }

        string mensaje = "¿Seguro que quiere eliminar el siguiente registro?\n\n" +
                         $"Clave: {carrera.IdCarrera}\n" +
                         $"Nombre: {carrera.Nombre}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar eliminacion"))
            return;

        await _repo.EliminarAsync(id);
        await Dialogos.MostrarAsync(this, "Registro Eliminado");
        txtClave.Text = "";
        txtNombre.Text = "";
    });
}
