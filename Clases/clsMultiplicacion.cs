using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsMultiplicacion
    {
        private float _valor1;
        private float _valor2;
        private float _resultado;

        public clsMultiplicacion(float valor1, float valor2)
        {
            _valor1 = valor1;
            _valor2 = valor2;
        }
        public float calcMulti()
        {
            _resultado = _valor1 * _valor2;
            return _resultado;
        }
    }
}
