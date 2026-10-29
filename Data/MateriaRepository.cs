using ControlEscolar.Models;
using Microsoft.Data.SqlClient;

namespace ControlEscolar.Data;

public class MateriaRepository
{
    private static Materia Mapear(SqlDataReader r) => new()
    {
        IdMateria = r.GetInt32(0),
        Nombre = r.GetString(1)
    };

    public async Task<Materia?> BuscarAsync(int id)
    {
        List<Materia> r = await Db.ConsultarAsync<Materia>(
            "SELECT iIdMateria, vNombre FROM tblMaterias WHERE iIdMateria = @id",
            Mapear, Db.Int("@id", id));
        return r.FirstOrDefault();
    }

    public Task InsertarAsync(Materia m) => Db.EjecutarAsync(
        "INSERT INTO tblMaterias (iIdMateria, vNombre) VALUES (@id, @nombre)",
        Db.Int("@id", m.IdMateria), Db.Texto("@nombre", m.Nombre, 100));

    public Task ModificarAsync(Materia m) => Db.EjecutarAsync(
        "UPDATE tblMaterias SET vNombre = @nombre WHERE iIdMateria = @id",
        Db.Texto("@nombre", m.Nombre, 100), Db.Int("@id", m.IdMateria));

    public Task EliminarAsync(int id) => Db.EjecutarAsync(
        "DELETE FROM tblMaterias WHERE iIdMateria = @id", Db.Int("@id", id));
}
