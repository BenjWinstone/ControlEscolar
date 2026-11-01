using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsCiudades
    {
        private int _idMunicipio;
        public clsCiudades()
        {
        }
        public clsCiudades(int idMunicipio)
        {
            _idMunicipio = idMunicipio;
        }
        public string ConsultaPorMunicipio()
        {
            return "SELECT * FROM tblCiudades WHERE iIdMunicipio = " + _idMunicipio;
        }
    }
}