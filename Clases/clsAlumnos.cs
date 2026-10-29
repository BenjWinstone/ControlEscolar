using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsAlumnos
    {
        private int _matricula;
        private string _nombre;
        private string _correo;
        private string _telefono;
        private int _idCarrera;
        public clsAlumnos()
        {
        }
        public clsAlumnos(int matricula, string nombre, string correo, string telefono, int idCarrera)
        {
            this._matricula = matricula;
            this._nombre = nombre;
            this._correo = correo;
            this._telefono = telefono;
            this._idCarrera = idCarrera;
        }
        public clsAlumnos(int matricula)
        {
            this._matricula = matricula;
        }
        public string Buscar()
        {
            return "SELECT * FROM tblAlumnos WHERE iMatricula = " + _matricula;
        }
        public string Guardar()
        {
            return $"INSERT INTO tblAlumnos (iMatricula, vNombre, vCorreo, vTelefono, iIdCarrera ) VALUES ({_matricula}, '{_nombre}', '{_correo}', '{_telefono}', {_idCarrera})";
        }
        public string Modificar()
        {
            return $"UPDATE tblAlumnos set vNombre = '{_nombre}', vCorreo = '{_correo}', vTelefono = '{_telefono}', iIdCarrera = {_idCarrera} where iMatricula = {_matricula}";
        }
        public string Eliminar()
        {
            return "DELETE FROM tblAlumnos WHERE iMatricula = " + _matricula;
        }
    }
}
