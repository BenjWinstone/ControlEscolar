using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsCiudades
    {
        private int _id;
        private int _idMunicipio;
        private string _nombre;
        public clsCiudades()
        {
        }
        // OJO: este constructor ya existia y recibe el MUNICIPIO (lo usa frmUnidades para llenar el combo)
        public clsCiudades(int idMunicipio)
        {
            _idMunicipio = idMunicipio;
        }
        // Para Buscar / Eliminar: idMunicipio se manda en 0 porque no se usa
        public clsCiudades(int id, int idMunicipio)
        {
            _id = id;
            _idMunicipio = idMunicipio;
        }
        public clsCiudades(int id, int idMunicipio, string nombre)
        {
            _id = id;
            _idMunicipio = idMunicipio;
            _nombre = nombre;
        }
        public string ConsultaPorMunicipio()
        {
            return "SELECT * FROM tblCiudades WHERE iIdMunicipio = " + _idMunicipio;
        }
        public string Guardar()
        {
            return $"insert into tblCiudades(iIdCiudad, iIdMunicipio, vCiudad) values ({_id},{_idMunicipio},'{_nombre}')";
        }
        public string Buscar()
        {
            // Sube de la ciudad al municipio para poder seleccionar los 2 combos (estado y municipio)
            return "SELECT c.iIdCiudad, c.vCiudad, c.iIdMunicipio, m.iIdEstado " +
                   "FROM tblCiudades c " +
                   "INNER JOIN tblMunicipios m ON c.iIdMunicipio = m.iIdMunicipio " +
                   "WHERE c.iIdCiudad = " + _id;
        }
        public string Modificar()
        {
            return $"update tblCiudades set vCiudad = '{_nombre}', iIdMunicipio = {_idMunicipio} where iIdCiudad = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblCiudades where iIdCiudad = {_id}";
        }
    }
}
