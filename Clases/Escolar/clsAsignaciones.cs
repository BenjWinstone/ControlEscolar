using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projMenuGeneral.Clases
{
    internal class clsAsignaciones
    {
        private int _id;
        private int _numeroEmpleado;
        private int _idMateria;
        private string _grupo;
        private string _salon;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private bool _vigente;
        public clsAsignaciones()
        {
        }
        public clsAsignaciones(int id)
        {
            _id = id;
        }
        public clsAsignaciones(int id, int numeroEmpleado, int idMateria, string grupo, string salon, DateTime fechaInicio, DateTime fechaFin, bool vigente)
        {
            _id = id;
            _numeroEmpleado = numeroEmpleado;
            _idMateria = idMateria;
            _grupo = grupo;
            _salon = salon;
            _fechaInicio = fechaInicio;
            _fechaFin = fechaFin;
            _vigente = vigente;
        }
        public string Guardar()
        {
            // El .Replace evita que una comilla simple en el texto libre rompa el INSERT (igual que clsSolicitudes)
            // Las fechas van en formato yyyyMMdd, que SQL Server entiende sin importar el idioma
            return $"insert into tblAsignaciones(iIdAsignacion, iNumeroEmpleado, iIdMateria, vGrupo, vSalon, dFechaInicio, dFechaFin, bVigente) " +
                   $"values ({_id},{_numeroEmpleado},{_idMateria},'{_grupo.Replace("'", "''")}','{_salon.Replace("'", "''")}','{_fechaInicio:yyyyMMdd}','{_fechaFin:yyyyMMdd}',{(_vigente ? 1 : 0)})";
        }
        public string Buscar()
        {
            // Trae tambien el nombre del maestro y de la materia para el mensaje de confirmacion
            return "SELECT a.iIdAsignacion, a.iNumeroEmpleado, a.iIdMateria, a.vGrupo, a.vSalon, a.dFechaInicio, a.dFechaFin, a.bVigente, " +
                   "m.vNombre AS vMaestro, t.vMateria " +
                   "FROM tblAsignaciones a " +
                   "INNER JOIN tblMaestros m ON a.iNumeroEmpleado = m.iNumeroEmpleado " +
                   "INNER JOIN tblMaterias t ON a.iIdMateria = t.iIdMateria " +
                   "WHERE a.iIdAsignacion = " + _id;
        }
        public string Modificar()
        {
            return $"update tblAsignaciones set iNumeroEmpleado = {_numeroEmpleado}, iIdMateria = {_idMateria}, vGrupo = '{_grupo.Replace("'", "''")}', vSalon = '{_salon.Replace("'", "''")}', " +
                   $"dFechaInicio = '{_fechaInicio:yyyyMMdd}', dFechaFin = '{_fechaFin:yyyyMMdd}', bVigente = {(_vigente ? 1 : 0)} where iIdAsignacion = {_id}";
        }
        public string Eliminar()
        {
            return $"delete from tblAsignaciones where iIdAsignacion = {_id}";
        }
    }
}