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
        private int _idCiudad;
        private int _id;
        public clsUnidades()
        {
        }
        public clsUnidades(int clave)
        {
            _id = clave;
        }
        public clsUnidades(int clave, string nombre, int idCiudad)
        {
            _id = clave;
            _nombre = nombre;
            _idCiudad = idCiudad;
        }
        public string Guardar()
        {
            return $"insert into tblUnidades(iIdUnidades, vNombre, iIdCiudad) values ({_id},'{_nombre}',{_idCiudad})";
        }
        public string Buscar()
        {
            // Sube de la ciudad al municipio y al estado, para poder seleccionar los 3 combos
            return "SELECT u.iIdUnidades, u.vNombre, u.iIdCiudad, c.vCiudad, c.iIdMunicipio, m.iIdEstado " +
                   "FROM tblUnidades u " +
                   "INNER JOIN tblCiudades c ON u.iIdCiudad = c.iIdCiudad " +
                   "INNER JOIN tblMunicipios m ON c.iIdMunicipio = m.iIdMunicipio " +
                   "WHERE u.iIdUnidades = " + _id;
        }
        public string Modificar()
        {
            return $"update tblUnidades set vNombre = '{_nombre}', iIdCiudad = {_idCiudad} where iIdUnidades = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblUnidades where iIdUnidades = {_id}";
        }
    }
}