using System.Globalization;

namespace ControlEscolar.Services;

/// <summary>Validaciones que antes estaban copiadas en cada formulario.</summary>
public static class Validaciones
{
    /// <summary>Solo digitos (sin signos ni espacios internos) y mayor que cero.</summary>
    public static bool EsEnteroPositivo(string? texto, out int valor) =>
        int.TryParse(texto?.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out valor) && valor > 0;

    public static bool EsCorreoValido(string correo)
    {
        if (correo.Contains(' '))
            return false;
        int arroba = correo.IndexOf('@');
        if (arroba <= 0)
            return false;
        if (arroba != correo.LastIndexOf('@'))
            return false;

        string dominio = correo[(arroba + 1)..];
        return dominio.Contains('.') && !dominio.StartsWith('.') && !dominio.EndsWith('.');
    }

    /// <summary>10 digitos ASCII. (char.IsDigit tambien acepta digitos de otros alfabetos.)</summary>
    public static bool EsTelefonoValido(string telefono) =>
        telefono.Length == 10 && telefono.All(c => c is >= '0' and <= '9');
}
