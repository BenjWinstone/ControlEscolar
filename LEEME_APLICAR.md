# Como aplicar estos cambios

Copia cada archivo a la MISMA ruta en tu proyecto (la estructura de carpetas es la misma).

1. Ejecuta `sql/01_crear_dbEscolar.sql` (script COMPLETO, reemplaza al anterior; crea todas las tablas, incluidas tblUsuarios y tblSolicitudes, y 2 usuarios: admin/admin y usuario/usuario). Es seguro correrlo sobre tu base actual: solo crea lo que falta.
2. `Clases/clsGlobal_AGREGAR.txt`: NO reemplaza nada, son lineas para pegar dentro de tu clsGlobal.cs.
3. Reemplazan archivos existentes: App.axaml.cs, frmMenuGeneral.axaml(.cs), todos los frm*.axaml de Escolar/, Operaciones/frmSuma.axaml,
   Escolar/frmDepartamentos.axaml(.cs), Clases/Escolar/Ubicaciones/clsEstados.cs, clsMunicipios.cs, clsCiudades.cs.
4. Archivos nuevos: frmLogin.*, Escolar/Ubicaciones/*, Escolar/Solicitudes/*, Clases/Escolar/clsUsuarios.cs, clsSolicitudes.cs.
5. Los .axaml.cs de Alumnos, Maestros, Materias, Carreras, Semestres, Turnos y Unidades NO cambian (se conservan los nombres de controles y handlers).
