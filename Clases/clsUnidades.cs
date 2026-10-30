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
        private string _ciudad;
        private string _municipio;
        private string _estado;
        private int _id;
        public clsUnidades()
        {
        }
        public clsUnidades(int clave)
        {
            _id = clave;
        }
        public clsUnidades(int clave, string nombre, string ciudad,string municipio, string estado)
        {
            _id = clave;
            _nombre = nombre;
            _ciudad = ciudad;
            _municipio = municipio;
            _estado = estado;
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
