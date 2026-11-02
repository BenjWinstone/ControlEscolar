using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsMunicipios
    {
        private int _id;
        private int _idEstado;
        private string _nombre;
        public clsMunicipios()
        {
        }
        // OJO: este constructor ya existia y recibe el ESTADO (lo usa frmUnidades para llenar el combo)
        public clsMunicipios(int idEstado)
        {
            _idEstado = idEstado;
        }
        // Para Buscar / Eliminar: idEstado se manda en 0 porque no se usa
        public clsMunicipios(int id, int idEstado)
        {
            _id = id;
            _idEstado = idEstado;
        }
        public clsMunicipios(int id, int idEstado, string nombre)
        {
            _id = id;
            _idEstado = idEstado;
            _nombre = nombre;
        }
        public string ConsultaPorEstado()
        {
            return "SELECT * FROM tblMunicipios WHERE iIdEstado = " + _idEstado;
        }
        public string Guardar()
        {
            return $"insert into tblMunicipios(iIdMunicipio, iIdEstado, vMunicipio) values ({_id},{_idEstado},'{_nombre}')";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblMunicipios where iIdMunicipio = " + _id;
        }
        public string Modificar()
        {
            return $"update tblMunicipios set vMunicipio = '{_nombre}', iIdEstado = {_idEstado} where iIdMunicipio = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblMunicipios where iIdMunicipio = {_id}";
        }
    }
}
