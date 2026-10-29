using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace ControlEscolar.Data;

/// <summary>
/// Acceso a SQL Server. Reemplaza a clsConexion + clsGlobal.
/// Todas las consultas reciben parametros (@nombre), nunca texto concatenado.
/// </summary>
public static class Db
{
    private static readonly Lazy<string> _cadena = new(CargarCadena);

    public static string CadenaConexion => _cadena.Value;

    private static string CargarCadena()
    {
        // Opcional: una variable de entorno tiene prioridad sobre el archivo.
        string? deEntorno = Environment.GetEnvironmentVariable("ESCOLAR_CONEXION");
        if (!string.IsNullOrWhiteSpace(deEntorno))
            return deEntorno;

        string ruta = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(ruta))
            throw new FileNotFoundException(
                "No se encontro appsettings.json. Copia appsettings.example.json como appsettings.json y escribe tu contrasena.",
                ruta);

        using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(ruta));
        string? valor = doc.RootElement.TryGetProperty("ConnectionString", out JsonElement el)
            ? el.GetString()
            : null;

        if (string.IsNullOrWhiteSpace(valor))
            throw new InvalidOperationException("appsettings.json no contiene la propiedad \"ConnectionString\".");

        return valor;
    }

    public static async Task<SqlConnection> AbrirAsync()
    {
        var conexion = new SqlConnection(CadenaConexion);
        try
        {
            await conexion.OpenAsync();
            return conexion;
        }
        catch
        {
            await conexion.DisposeAsync();
            throw;
        }
    }

    /// <summary>INSERT / UPDATE / DELETE. Devuelve las filas afectadas.</summary>
    public static async Task<int> EjecutarAsync(string sql, params SqlParameter[] parametros)
    {
        await using SqlConnection cn = await AbrirAsync();
        await using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        return await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>SELECT. Convierte cada fila con la funcion "mapa".</summary>
    public static async Task<List<T>> ConsultarAsync<T>(string sql, Func<SqlDataReader, T> mapa, params SqlParameter[] parametros)
    {
        var lista = new List<T>();
        await using SqlConnection cn = await AbrirAsync();
        await using var cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        await using SqlDataReader lector = await cmd.ExecuteReaderAsync();
        while (await lector.ReadAsync())
            lista.Add(mapa(lector));
        return lista;
    }

    public static SqlParameter Int(string nombre, int valor) =>
        new SqlParameter(nombre, SqlDbType.Int) { Value = valor };

    public static SqlParameter Texto(string nombre, string valor, int largo) =>
        new SqlParameter(nombre, SqlDbType.VarChar, largo) { Value = valor };
}
