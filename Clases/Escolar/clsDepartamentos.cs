using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsDepartamentos
    {
        private string _nombre;
        private int _id;
        public clsDepartamentos()
        {
        }
        public clsDepartamentos(int id)
        {
            _id = id;
        }
        public clsDepartamentos(int id, string nombre)
        {
            _id = id;
            _nombre = nombre;
        }
        public string Guardar()
        {
            return $"insert into tblDepartamentos(iIdDepartamento, iValorTurnos) values ({_id},'{_nombre}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblDepartamentos where iIdDepartamento = " + _id;
        }
        public string Modificar()
        {
            return $"update tblDepartamentos set iValorTurnos = '{_nombre}' where iIdDepartamento = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblDepartamentos where iIdDepartamento = {_id}";
        }
    }
}