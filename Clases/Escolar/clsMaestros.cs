using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsMaestros
    {
        private int _numeroEmpleado;
        private string _nombre;
        private string _telefono;
        private string _correo;
        private int _idCarrera;

        public clsMaestros()
        {
        }
        public clsMaestros(int numeroEmpleado, string nombre, string telefono, string correo, int carrera)
        {
            this._numeroEmpleado = numeroEmpleado;
            this._nombre = nombre;
            this._telefono = telefono;
            this._correo = correo;
            this._idCarrera = carrera;
        }
        public clsMaestros(int numeroEmpleado)
        {
            this._numeroEmpleado= numeroEmpleado;
        }
        public string Buscar()
        {
            return "SELECT * FROM tblMaestros WHERE iNumeroEmpleado = " + _numeroEmpleado;
        }
        public string Guardar()
        {
            return $"INSERT INTO tblMaestros (iNumeroEmpleado, vNombre, vCorreo, vTelefono, iIdCarrera) VALUES ({_numeroEmpleado}, '{_nombre}', '{_correo}', '{_telefono}', {_idCarrera})";
        }
        public string Modificar()
        {
            return $"UPDATE tblMaestros set vNombre = '{_nombre}', vCorreo = '{_correo}', vTelefono = '{_telefono}', iIdCarrera = {_idCarrera} where iNumeroEmpleado = {_numeroEmpleado}";
        }
        public string Eliminar()
        {
            return "DELETE FROM tblMaestros WHERE iNumeroEmpleado = " + _numeroEmpleado;
        }
    }
}
