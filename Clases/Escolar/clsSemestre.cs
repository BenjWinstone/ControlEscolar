using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsSemestre
    {
        private int _nivel;
        private int _clave;
        public clsSemestre()
        {
        }
        public clsSemestre(int clave)
        {
            _clave = clave;
        }
        public clsSemestre(int clave, int nivel)
        {
            _clave = clave;
            _nivel = nivel;
        }
        public string Guardar()
        {
            return $"insert into tblSemestres(iIdSemestre, iNivel) values ({_clave},'{_nivel}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblSemestres where iIdSemestre = " + _clave;
        }
        public string Modificar()
        {
            return $"update tblSemestres set iNivel = '{_nivel}' where iIdSemestre = {_clave}";
        }
        public string Eliminar()
        {
            return $"delete from tblSemestres where iIdSemestre = {_clave}";
        }
    }
}
