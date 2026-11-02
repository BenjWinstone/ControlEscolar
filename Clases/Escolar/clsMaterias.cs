using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsMaterias
    {
        private string _nombre;
        private int _clave;
        public clsMaterias()
        {
        }
        public clsMaterias(int clave)
        {
            _clave = clave;
        }
        public clsMaterias(int clave, string nombre)
        {
            _clave = clave;
            _nombre = nombre;
        }
        public string Guardar()
        {
            return $"insert into tblMaterias(iIdMateria, vMateria) values ({_clave},'{_nombre}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblMaterias where iIdMateria = " + _clave;
        }
        public string Modificar()
        {
            return $"update tblMaterias set vMateria = '{_nombre}' where iIdMateria = {_clave}";
        }
        public string Eliminar()
        {
            return $"delete from tblMaterias where iIdMateria = {_clave}";
        }
    }
}
