using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsEstados
    {
        public clsEstados()
        {
        }
        public string ConsultaGeneral()
        {
            return "SELECT * FROM tblEstados";
        }
    }
}