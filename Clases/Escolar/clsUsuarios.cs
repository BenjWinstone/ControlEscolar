using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsUsuarios
    {
        private string _usuario;
        private string _contrasena;
        public clsUsuarios()
        {
        }
        public clsUsuarios(string usuario, string contrasena)
        {
            _usuario = usuario;
            _contrasena = contrasena;
        }
        public string Login()
        {
            return $"SELECT iIdUsuario, vUsuario, iRol FROM tblUsuarios WHERE vUsuario = '{_usuario}' AND vContrasena = '{_contrasena}'";
        }
    }
}
