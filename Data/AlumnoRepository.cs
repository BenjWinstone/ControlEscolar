using ControlEscolar.Models;
using Microsoft.Data.SqlClient;

namespace ControlEscolar.Data;

public class AlumnoRepository
{
    private static Alumno Mapear(SqlDataReader r) => new()
    {
        Matricula = r.GetInt32(0),
        Nombre = r.GetString(1),
        Correo = r.GetString(2),
        Telefono = r.GetString(3),
        IdCarrera = r.GetInt32(4)
    };

    public async Task<Alumno?> BuscarAsync(int matricula)
    {
        List<Alumno> r = await Db.ConsultarAsync<Alumno>(
            "SELECT iMatricula, vNombre, vCorreo, vTelefono, iIdCarrera FROM tblAlumnos WHERE iMatricula = @matricula",
            Mapear, Db.Int("@matricula", matricula));
        return r.FirstOrDefault();
    }

    public Task InsertarAsync(Alumno a) => Db.EjecutarAsync(
        "INSERT INTO tblAlumnos (iMatricula, vNombre, vCorreo, vTelefono, iIdCarrera) " +
        "VALUES (@matricula, @nombre, @correo, @telefono, @idCarrera)",
        Db.Int("@matricula", a.Matricula),
        Db.Texto("@nombre", a.Nombre, 100),
        Db.Texto("@correo", a.Correo, 100),
        Db.Texto("@telefono", a.Telefono, 10),
        Db.Int("@idCarrera", a.IdCarrera));

    public Task ModificarAsync(Alumno a) => Db.EjecutarAsync(
        "UPDATE tblAlumnos SET vNombre = @nombre, vCorreo = @correo, vTelefono = @telefono, iIdCarrera = @idCarrera " +
        "WHERE iMatricula = @matricula",
        Db.Texto("@nombre", a.Nombre, 100),
        Db.Texto("@correo", a.Correo, 100),
        Db.Texto("@telefono", a.Telefono, 10),
        Db.Int("@idCarrera", a.IdCarrera),
        Db.Int("@matricula", a.Matricula));

    public Task EliminarAsync(int matricula) => Db.EjecutarAsync(
        "DELETE FROM tblAlumnos WHERE iMatricula = @matricula", Db.Int("@matricula", matricula));
}
