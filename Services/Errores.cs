using Microsoft.Data.SqlClient;

namespace ControlEscolar.Services;

/// <summary>Convierte excepciones en mensajes entendibles para el usuario.</summary>
public static class Errores
{
    public static string Mensaje(Exception ex) => ex switch
    {
        SqlException { Number: 547 } =>
            "La operacion fue rechazada por una relacion con otros registros " +
            "(por ejemplo: la carrera no existe, o tiene alumnos o maestros asociados).",
        SqlException { Number: 2627 or 2601 } =>
            "Ya existe un registro con esa clave.",
        SqlException =>
            "No se pudo completar la operacion con la base de datos. " +
            "Verifica que el contenedor de SQL Server este en ejecucion y que appsettings.json sea correcto.\n\n" +
            "Detalle: " + ex.Message,
        FileNotFoundException or InvalidOperationException =>
            ex.Message,
        _ => "Ocurrio un error inesperado: " + ex.Message
    };
}
