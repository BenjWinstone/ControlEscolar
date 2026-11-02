using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsEstados
    {
        private int _id;
        private string _nombre;
        public clsEstados()
        {
        }
        public clsEstados(int id)
        {
            _id = id;
        }
        public clsEstados(int id, string nombre)
        {
            _id = id;
            _nombre = nombre;
        }
        public string ConsultaGeneral()
        {
            return "SELECT * FROM tblEstados";
        }
        public string Guardar()
        {
            return $"insert into tblEstados(iIdEstado, vEstado) values ({_id},'{_nombre}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblEstados where iIdEstado = " + _id;
        }
        public string Modificar()
        {
            return $"update tblEstados set vEstado = '{_nombre}' where iIdEstado = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblEstados where iIdEstado = {_id}";
        }
    }
}
