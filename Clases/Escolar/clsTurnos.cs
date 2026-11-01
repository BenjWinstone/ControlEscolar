using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsTurnos
    {
        private string _nombre;
        private int _id;
        public clsTurnos()
        {
        }
        public clsTurnos(int id)
        {
            _id = id;
        }
        public clsTurnos(int id, string nombre)
        {
            _id = id;
            _nombre = nombre;
        }
        public string Guardar()
        {
            return $"insert into tblTurnos(iIdTurnos, iValorTurnos) values ({_id},'{_nombre}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblTurnos where iIdTurnos = " + _id;
        }
        public string Modificar()
        {
            return $"update tblTurnos set iValorTurnos = '{_nombre}' where iIdTurnos = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblTurnos where iIdTurnos = {_id}";
        }
    }
}
