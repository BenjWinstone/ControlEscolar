using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient; // antes: System.Data.SqlClient (mismas clases, paquete actual)
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsConexion
    {
        private string _sentencia;
        private SqlConnection _conn = new SqlConnection();
        private SqlCommand _cmd;

        public clsConexion()
        {
        }
        public clsConexion(string sentencia)
        {
            _sentencia = sentencia;
        }
        public string Ejecutar()
        { //Guardar, Modificar y Eliminar
            _conn.ConnectionString = Clases.clsGlobal.conexion;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Close();
                _conn.Open();
            }
            _cmd = new SqlCommand();
            _cmd.Connection = _conn;
            _cmd.CommandText = _sentencia;
            _cmd.ExecuteNonQuery();
            _conn.Close();
            return "Operacion Exitosa";
        }
        public DataSet Consultar()
        { //Consultas
            DataSet datos = new DataSet();
            _conn.ConnectionString = Clases.clsGlobal.conexion;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Close();
                _conn.Open();
            }

            SqlDataAdapter resp = new SqlDataAdapter(_sentencia, _conn);
            resp.Fill(datos, "Tabla");
            _conn.Close();
            return datos;
        }
    }
}
