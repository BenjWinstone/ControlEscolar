using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace projMenuGeneral.Clases
{
    internal class clsGlobal
    {
        static public string _dbn = "dbEscolar";
        //static public string _server = @"void\sqlserver2025";   // original (Windows)
        static public string _server = "localhost,1433";            // contenedor de SQL Server en Fedora (cambia el puerto si es otro)
        //static public string _psswd = "1234";                      // original
        static public string _psswd = "Liz.142857";
        //static public string _sec = "Integrated Security=True";   // original: autenticacion de Windows, no existe en Linux
        static public string _sec = "TrustServerCertificate=True";   // el contenedor usa un certificado propio
        static public string _UserID = "sa";
        static public string conexion = $"Data Source={_server}; Initial Catalog={_dbn};User ID={_UserID};Password={_psswd};{_sec}";
            //$"Data Source={_server};Initial Catalog={_dbn};Persist Security Info=True;User ID={_UserID};Password={_psswd};Encrypt=False;Trusted_Connection=True;"; 

        // Reemplazo de Application.Exit() de WinForms: cierra todo el programa
        static public void Salir()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.Shutdown();
        }
    }
}
