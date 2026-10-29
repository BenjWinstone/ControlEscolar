using ControlEscolar.Models;
using Microsoft.Data.SqlClient;

namespace ControlEscolar.Data;

public class MaestroRepository
{
    private static Maestro Mapear(SqlDataReader r) => new()
    {
        NumeroEmpleado = r.GetInt32(0),
        Nombre = r.GetString(1),
        Correo = r.GetString(2),
        Telefono = r.GetString(3),
        IdCarrera = r.GetInt32(4)
    };

    public async Task<Maestro?> BuscarAsync(int numeroEmpleado)
    {
        List<Maestro> r = await Db.ConsultarAsync<Maestro>(
            "SELECT iNumeroEmpleado, vNombre, vCorreo, vTelefono, iIdCarrera FROM tblMaestros WHERE iNumeroEmpleado = @numero",
            Mapear, Db.Int("@numero", numeroEmpleado));
        return r.FirstOrDefault();
    }

    public Task InsertarAsync(Maestro m) => Db.EjecutarAsync(
        "INSERT INTO tblMaestros (iNumeroEmpleado, vNombre, vCorreo, vTelefono, iIdCarrera) " +
        "VALUES (@numero, @nombre, @correo, @telefono, @idCarrera)",
        Db.Int("@numero", m.NumeroEmpleado),
        Db.Texto("@nombre", m.Nombre, 100),
        Db.Texto("@correo", m.Correo, 100),
        Db.Texto("@telefono", m.Telefono, 10),
        Db.Int("@idCarrera", m.IdCarrera));

    public Task ModificarAsync(Maestro m) => Db.EjecutarAsync(
        "UPDATE tblMaestros SET vNombre = @nombre, vCorreo = @correo, vTelefono = @telefono, iIdCarrera = @idCarrera " +
        "WHERE iNumeroEmpleado = @numero",
        Db.Texto("@nombre", m.Nombre, 100),
        Db.Texto("@correo", m.Correo, 100),
        Db.Texto("@telefono", m.Telefono, 10),
        Db.Int("@idCarrera", m.IdCarrera),
        Db.Int("@numero", m.NumeroEmpleado));

    public Task EliminarAsync(int numeroEmpleado) => Db.EjecutarAsync(
        "DELETE FROM tblMaestros WHERE iNumeroEmpleado = @numero", Db.Int("@numero", numeroEmpleado));
}
