using Avalonia.Controls;
using ControlEscolar.Data;
using ControlEscolar.Models;
using ControlEscolar.Services;

namespace ControlEscolar.Views;

public partial class MateriasWindow : Window
{
    private readonly MateriaRepository _repo = new();

    public MateriasWindow()
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

        Materia? materia = await _repo.BuscarAsync(id);
        if (materia == null)
        {
            txtNombre.Text = "";
            await Dialogos.MostrarAsync(this, "No existe esa materia");
            return;
        }
        txtNombre.Text = materia.Nombre;
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
            await Dialogos.AvisarYEnfocarAsync(this, "Escriba el nombre de la materia", txtNombre);
            return;
        }

        var nueva = new Materia { IdMateria = id, Nombre = nombre };
        Materia? existente = await _repo.BuscarAsync(id);

        if (existente == null)
        {
            await _repo.InsertarAsync(nueva);
            await Dialogos.MostrarAsync(this, "Registro Realizado con exito");
            return;
        }

        string mensaje = "¿Seguro que quiere actualizar el siguiente registro?\n\n" +
                         $"Clave: {existente.IdMateria}\n" +
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

        Materia? materia = await _repo.BuscarAsync(id);
        if (materia == null)
        {
            await Dialogos.MostrarAsync(this, "No existe esa materia");
            return;
        }

        string mensaje = "¿Seguro que quiere eliminar el siguiente registro?\n\n" +
                         $"Clave: {materia.IdMateria}\n" +
                         $"Nombre: {materia.Nombre}";
        if (!await Dialogos.ConfirmarAsync(this, mensaje, "Confirmar eliminacion"))
            return;

        await _repo.EliminarAsync(id);
        await Dialogos.MostrarAsync(this, "Registro Eliminado");
        txtClave.Text = "";
        txtNombre.Text = "";
    });
}
