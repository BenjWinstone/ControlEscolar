using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsUnidades
    {
        private string _nombre;
        private string _municipio;
        private int _id;
        public clsUnidades()
        {
        }
        public clsUnidades(int clave)
        {
            _id = clave;
        }
        public clsUnidades(int clave, string nombre, string municipio)
        {
            _id = clave;
            _nombre = nombre;
            _municipio = municipio;
        }
        public string Guardar()
        {
            return $"insert into tblUnidades(iIdUnidades, iValorUnidades) values ({_id},'{_nombre}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblUnidades where iIdUnidades = " + _id;
        }
        public string Modificar()
        {
            return $"update tblUnidades set iValorUnidades = '{_nombre}' where iIdUnidades = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblUnidades where iIdUnidades = {_id}";
        }
    }
}
