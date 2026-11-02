using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsDepartamentos
    {
        private string _departamento;
        private byte _id;
        public clsDepartamentos()
        {
        }
        public clsDepartamentos(byte id)
        {
            _id = id;
        }
        public clsDepartamentos(byte id, string nombre)
        {
            _id = id;
            _departamento = nombre;
        }
        public string ConsultaGeneral()
        {
            return "SELECT * FROM tblDepartamentos";
        }
        public string Guardar()
        {
            return $"insert into tblDepartamentos(iIdDepartamento, vDepartamento) values ({_id},'{_departamento}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblDepartamentos where iIdDepartamento = " + _id;
        }
        public string Modificar()
        {
            return $"update tblDepartamentos set vDepartamento = '{_departamento}' where iIdDepartamento = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblDepartamentos where iIdDepartamento = {_id}";
        }
    }
}