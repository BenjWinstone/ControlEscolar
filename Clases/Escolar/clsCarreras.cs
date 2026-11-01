using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsCarreras
    {
        private int _idCarrera;
        private string _nombreCarrera;
        private byte _idDepartamento;
        public clsCarreras()
        {
        }
        public clsCarreras(int idCarrera)
        {
            this._idCarrera = idCarrera;
        }
        public clsCarreras(int idCarrera, string nombreCarrera) // CAMBIO: constructor que usa frmCarreras (en tu version estaba quitado)
        {
            this._idCarrera = idCarrera;
            this._nombreCarrera = nombreCarrera;
        }
        public clsCarreras(int idCarrera, string nombreCarrera, byte idDepartamento) // CAMBIO: faltaba el tipo "byte"
        {
            this._idCarrera = idCarrera;
            this._nombreCarrera = nombreCarrera;
            this._idDepartamento = idDepartamento;
        }
        public string ConsultaGeneral()
        {
            return "SELECT * FROM tblCarreras";
        }
        public string Buscar()
        {
            return "SELECT * FROM tblCarreras WHERE iIdCarrera= " + _idCarrera;
        }
        public string Guardar()
        {
            return $"INSERT INTO tblCarreras (iIdCarrera, vCarrera, iIdDepartamento) VALUES ({_idCarrera}, '{_nombreCarrera}', {_idDepartamento})";
        }
        public string Modificar()
        {
            return $"UPDATE tblCarreras set vCarrera= '{_nombreCarrera}', iIdDepartamento = {_idDepartamento} where iIdCarrera = {_idCarrera}";
        }
        public string Eliminar()
        {
            return "DELETE FROM tblCarreras WHERE iIdCarrera = " + _idCarrera;
        }
    }
}
