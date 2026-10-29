using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    // NUEVA (Avalonia): el ComboBox de WinForms tenia DataSource/DisplayMember/ValueMember
    // para mostrar un texto (vCarrera) y guardar un valor (iIdCarrera). El ComboBox de Avalonia
    // no los tiene, asi que cada opcion guarda las dos cosas en un objeto.
    internal class clsOpcion
    {
        private int _valor;
        private string _texto;

        public clsOpcion(int valor, string texto)
        {
            _valor = valor;
            _texto = texto;
        }
        public int Valor
        {
            get { return _valor; }
        }
        public string Texto
        {
            get { return _texto; }
        }
        // El ComboBox muestra lo que devuelva ToString()
        public override string ToString()
        {
            return _texto;
        }
    }
}
