using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsSolicitudes
    {
        private int _id;
        private string _titulo;
        private string _descripcion;
        private int _idUsuario;
        public clsSolicitudes()
        {
        }
        public clsSolicitudes(int id)
        {
            _id = id;
        }
        public clsSolicitudes(string titulo, string descripcion, int idUsuario)
        {
            _titulo = titulo;
            _descripcion = descripcion;
            _idUsuario = idUsuario;
        }
        public string Guardar()
        {
            // El .Replace evita que una comilla simple en el texto libre (d'Angelo) rompa el INSERT
            return $"INSERT INTO tblSolicitudes (vTitulo, vDescripcion, iIdUsuario) VALUES ('{_titulo.Replace("'", "''")}', '{_descripcion.Replace("'", "''")}', {_idUsuario})";
        }
        public string ConsultaPendientes()
        {
            return "SELECT s.iIdSolicitud, s.vTitulo, s.vDescripcion, s.dFecha, u.vUsuario " +
                   "FROM tblSolicitudes s " +
                   "INNER JOIN tblUsuarios u ON s.iIdUsuario = u.iIdUsuario " +
                   "WHERE s.bAtendida = 0 ORDER BY s.dFecha";
        }
        public string MarcarAtendida()
        {
            return "UPDATE tblSolicitudes SET bAtendida = 1 WHERE iIdSolicitud = " + _id;
        }
    }
}
