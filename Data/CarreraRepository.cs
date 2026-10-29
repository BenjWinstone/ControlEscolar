using ControlEscolar.Models;
using Microsoft.Data.SqlClient;

namespace ControlEscolar.Data;

public class CarreraRepository
{
    private static Carrera Mapear(SqlDataReader r) => new()
    {
        IdCarrera = r.GetInt32(0),
        Nombre = r.GetString(1)
    };

    public Task<List<Carrera>> ListarAsync() =>
        Db.ConsultarAsync<Carrera>("SELECT iIdCarrera, vCarrera FROM tblCarreras ORDER BY vCarrera", Mapear);

    public async Task<Carrera?> BuscarAsync(int id)
    {
        List<Carrera> r = await Db.ConsultarAsync<Carrera>(
            "SELECT iIdCarrera, vCarrera FROM tblCarreras WHERE iIdCarrera = @id",
            Mapear, Db.Int("@id", id));
        return r.FirstOrDefault();
    }

    public Task InsertarAsync(Carrera c) => Db.EjecutarAsync(
        "INSERT INTO tblCarreras (iIdCarrera, vCarrera) VALUES (@id, @nombre)",
        Db.Int("@id", c.IdCarrera), Db.Texto("@nombre", c.Nombre, 100));

    public Task ModificarAsync(Carrera c) => Db.EjecutarAsync(
        "UPDATE tblCarreras SET vCarrera = @nombre WHERE iIdCarrera = @id",
        Db.Texto("@nombre", c.Nombre, 100), Db.Int("@id", c.IdCarrera));

    public Task EliminarAsync(int id) => Db.EjecutarAsync(
        "DELETE FROM tblCarreras WHERE iIdCarrera = @id", Db.Int("@id", id));
}
