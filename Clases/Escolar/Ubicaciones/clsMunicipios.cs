using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsMunicipios
    {
        private int _idEstado;
        public clsMunicipios()
        {
        }
        public clsMunicipios(int idEstado)
        {
            _idEstado = idEstado;
        }
        public string ConsultaPorEstado()
        {
            return "SELECT * FROM tblMunicipios WHERE iIdEstado = " + _idEstado;
        }
    }
}