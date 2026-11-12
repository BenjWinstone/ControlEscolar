/* =====================================================================
   dbEscolar - Script de creacion COMPLETO (proyecto projMenuGeneral)
   Actualizado a partir de los scripts exportados de la base de datos real
   (12/11/2026).

   COMO USAR ESTE ARCHIVO
   - Se puede ejecutar completo las veces que quieras: cada bloque pregunta
     primero si el objeto ya existe y, si existe, no lo toca.
   - Para AGREGAR UNA TABLA NUEVA: copia un bloque "IF OBJECT_ID ... CREATE TABLE"
     al final de la seccion 1. Si la tabla tiene llave foranea, va DESPUES de la
     tabla que referencia.
   - Para CAMBIAR UNA TABLA QUE YA EXISTE: no edites su CREATE TABLE (no se
     ejecutaria en una base que ya la tiene). Agrega un bloque en la seccion 3:
         IF COL_LENGTH(N'dbo.tblX', N'columna') IS NULL
             ALTER TABLE dbo.tblX ADD columna TIPO NULL;
         GO   <-- solo en su propia linea
   - Los GO separan lotes: despues de crear/alterar algo, lo que use ese cambio
     va en el lote siguiente.

   ORDEN DE CREACION (por las llaves foraneas)
       tblDepartamentos -> tblCarreras -> tblAlumnos, tblMaestros
       tblEstados -> tblMunicipios -> tblCiudades -> tblUnidades
       tblMaterias, tblSemestres, tblTurnos   (sin dependencias)
       tblUsuarios -> tblSolicitudes          (login y solicitudes)
       tblMaestros + tblMaterias -> tblAsignaciones
       tblUnidades + tblDepartamentos -> tblUnidadDepartamento

   NOTAS
   - Las tablas de la seccion 1 coinciden con los scripts exportados de la base
     (columnas, tipos, NOT NULL, PK, CHECK, UNIQUE, FK y DEFAULT).
   - tblAsignaciones no venia entre los scripts exportados: revisa si ya existe
     en tu base real.
   ===================================================================== */

IF DB_ID(N'dbEscolar') IS NULL
    CREATE DATABASE dbEscolar;
GO

USE dbEscolar;
GO

/* =============== 1. TABLAS =============== */

/* ---------- tblDepartamentos ---------- */
IF OBJECT_ID(N'dbo.tblDepartamentos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblDepartamentos
    (
        iIdDepartamento TINYINT      NOT NULL,
        vDepartamento   VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblDepartamentos PRIMARY KEY (iIdDepartamento),
        CONSTRAINT CK_tblDepartamentos_iIdDepartamento CHECK (iIdDepartamento > 0)
    );
END
GO

/* ---------- tblCarreras ---------- */
IF OBJECT_ID(N'dbo.tblCarreras', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblCarreras
    (
        iIdCarrera      INT          NOT NULL,
        vCarrera        VARCHAR(100) NOT NULL,
        iIdDepartamento TINYINT      NOT NULL,
        CONSTRAINT PK_tblCarreras PRIMARY KEY (iIdCarrera),
        CONSTRAINT CK_tblCarreras_iIdCarrera CHECK (iIdCarrera > 0),
        CONSTRAINT CK_tblCarreras_iIdDepartamento CHECK (iIdDepartamento > 0),
        CONSTRAINT FK_tblCarreras_tblDepartamentos FOREIGN KEY (iIdDepartamento) REFERENCES dbo.tblDepartamentos (iIdDepartamento)
    );
END
GO

/* ---------- tblAlumnos ---------- */
IF OBJECT_ID(N'dbo.tblAlumnos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblAlumnos
    (
        iMatricula INT          NOT NULL,
        vNombre    VARCHAR(100) NOT NULL,
        vCorreo    VARCHAR(100) NOT NULL,
        vTelefono  VARCHAR(10)  NOT NULL,
        iIdCarrera INT          NOT NULL,
        CONSTRAINT PK_tblAlumnos PRIMARY KEY (iMatricula),
        CONSTRAINT CK_tblAlumnos_iMatricula CHECK (iMatricula > 0),
        CONSTRAINT CK_tblAlumnos_vTelefono CHECK (LEN(vTelefono) = 10 AND vTelefono NOT LIKE '%[^0-9]%'),
        CONSTRAINT FK_tblAlumnos_tblCarreras FOREIGN KEY (iIdCarrera) REFERENCES dbo.tblCarreras (iIdCarrera)
    );
END
GO

/* ---------- tblMaestros ---------- */
IF OBJECT_ID(N'dbo.tblMaestros', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblMaestros
    (
        iNumeroEmpleado INT          NOT NULL,
        vNombre         VARCHAR(100) NOT NULL,
        vCorreo         VARCHAR(100) NOT NULL,
        vTelefono       VARCHAR(10)  NOT NULL,
        iIdCarrera      INT          NOT NULL,
        CONSTRAINT PK_tblMaestros PRIMARY KEY (iNumeroEmpleado),
        CONSTRAINT CK_tblMaestros_iNumeroEmpleado CHECK (iNumeroEmpleado > 0),
        CONSTRAINT CK_tblMaestros_vTelefono CHECK (LEN(vTelefono) = 10 AND vTelefono NOT LIKE '%[^0-9]%'),
        CONSTRAINT FK_tblMaestros_tblCarreras FOREIGN KEY (iIdCarrera) REFERENCES dbo.tblCarreras (iIdCarrera)
    );
END
GO

/* ---------- tblMaterias ---------- */
IF OBJECT_ID(N'dbo.tblMaterias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblMaterias
    (
        iIdMateria INT          NOT NULL,
        vMateria   VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblMaterias PRIMARY KEY (iIdMateria),
        CONSTRAINT CK_tblMaterias_iIdMateria CHECK (iIdMateria > 0)
    );
END
GO

/* ---------- tblSemestres ---------- */
IF OBJECT_ID(N'dbo.tblSemestres', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblSemestres
    (
        iIdSemestre INT NOT NULL,
        iNivel      INT NOT NULL,
        CONSTRAINT PK_tblSemestres PRIMARY KEY (iIdSemestre),
        CONSTRAINT CK_tblSemestres_iIdSemestre CHECK (iIdSemestre > 0)
    );
END
GO

/* ---------- tblTurnos ---------- */
IF OBJECT_ID(N'dbo.tblTurnos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblTurnos
    (
        iIdTurnos INT         NOT NULL,
        vTurno    VARCHAR(50) NOT NULL,
        CONSTRAINT PK_tblTurnos PRIMARY KEY (iIdTurnos),
        CONSTRAINT CK_tblTurnos_iIdTurnos CHECK (iIdTurnos > 0)
    );
END
GO

/* ---------- tblEstados ---------- */
IF OBJECT_ID(N'dbo.tblEstados', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblEstados
    (
        iIdEstado INT          NOT NULL,
        vEstado   VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblEstados PRIMARY KEY (iIdEstado),
        CONSTRAINT CK_tblEstados_iIdEstado CHECK (iIdEstado > 0)
    );
END
GO

/* ---------- tblMunicipios ---------- */
IF OBJECT_ID(N'dbo.tblMunicipios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblMunicipios
    (
        iIdMunicipio INT          NOT NULL,
        iIdEstado    INT          NOT NULL,
        vMunicipio   VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblMunicipios PRIMARY KEY (iIdMunicipio),
        CONSTRAINT CK_tblMunicipios_iIdMunicipio CHECK (iIdMunicipio > 0),
        CONSTRAINT CK_tblMunicipios_iIdEstado CHECK (iIdEstado > 0),
        CONSTRAINT FK_tblMunicipios_tblEstados FOREIGN KEY (iIdEstado) REFERENCES dbo.tblEstados (iIdEstado)
    );
END
GO

/* ---------- tblCiudades ---------- */
IF OBJECT_ID(N'dbo.tblCiudades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblCiudades
    (
        iIdCiudad    INT          NOT NULL,
        iIdMunicipio INT          NOT NULL,
        vCiudad      VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblCiudades PRIMARY KEY (iIdCiudad),
        CONSTRAINT CK_tblCiudades_iIdCiudad CHECK (iIdCiudad > 0),
        CONSTRAINT CK_tblCiudades_iIdMunicipio CHECK (iIdMunicipio > 0),
        CONSTRAINT FK_tblCiudades_tblMunicipios FOREIGN KEY (iIdMunicipio) REFERENCES dbo.tblMunicipios (iIdMunicipio)
    );
END
GO

/* ---------- tblUnidades ---------- */
IF OBJECT_ID(N'dbo.tblUnidades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblUnidades
    (
        iIdUnidades INT         NOT NULL,
        vNombre     VARCHAR(50) NOT NULL,
        iIdCiudad   INT         NOT NULL,
        CONSTRAINT PK_tblUnidades PRIMARY KEY (iIdUnidades),
        CONSTRAINT CK_tblUnidades_iIdUnidades CHECK (iIdUnidades > 0),
        CONSTRAINT FK_tblUnidades_tblCiudades FOREIGN KEY (iIdCiudad) REFERENCES dbo.tblCiudades (iIdCiudad)
    );
END
GO

/* ---------- tblUsuarios (login) ----------
   iRol: 1 = usuario, 2 = admin */
IF OBJECT_ID(N'dbo.tblUsuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblUsuarios
    (
        iIdUsuario  INT         NOT NULL,
        vUsuario    VARCHAR(50) NOT NULL,
        vContrasena VARCHAR(50) NOT NULL,
        iRol        TINYINT     NOT NULL,
        CONSTRAINT PK_tblUsuarios PRIMARY KEY (iIdUsuario),
        CONSTRAINT UQ_tblUsuarios_vUsuario UNIQUE (vUsuario),
        CONSTRAINT CK_tblUsuarios_iIdUsuario CHECK (iIdUsuario > 0),
        CONSTRAINT CK_tblUsuarios_iRol CHECK (iRol IN (1, 2))
    );
END
GO

/* ---------- tblSolicitudes (solicitudes de lugares nuevos) ---------- */
IF OBJECT_ID(N'dbo.tblSolicitudes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblSolicitudes
    (
        iIdSolicitud INT IDENTITY(1,1) NOT NULL,
        vTitulo      VARCHAR(100) NOT NULL,
        vDescripcion VARCHAR(500) NOT NULL,
        iIdUsuario   INT          NOT NULL,
        dFecha       DATETIME     NOT NULL CONSTRAINT DF_tblSolicitudes_dFecha DEFAULT (GETDATE()),
        bAtendida    BIT          NOT NULL CONSTRAINT DF_tblSolicitudes_bAtendida DEFAULT (0),
        CONSTRAINT PK_tblSolicitudes PRIMARY KEY (iIdSolicitud),
        CONSTRAINT FK_tblSolicitudes_tblUsuarios FOREIGN KEY (iIdUsuario) REFERENCES dbo.tblUsuarios (iIdUsuario)
    );
END
GO

/* ---------- tblAsignaciones (maestros <-> materias, muchos a muchos) ----------
   Va DESPUES de tblMaestros y tblMaterias.
   bVigente: 1 = vigente, 0 = terminada */
IF OBJECT_ID(N'dbo.tblAsignaciones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblAsignaciones
    (
        iIdAsignacion   INT         NOT NULL,
        iNumeroEmpleado INT         NOT NULL,
        iIdMateria      INT         NOT NULL,
        vGrupo          VARCHAR(10) NOT NULL,
        vSalon          VARCHAR(10) NOT NULL,
        dFechaInicio    DATE        NOT NULL,
        dFechaFin       DATE        NOT NULL,
        bVigente        BIT         NOT NULL,
        CONSTRAINT PK_tblAsignaciones PRIMARY KEY (iIdAsignacion),
        CONSTRAINT CK_tblAsignaciones_iIdAsignacion CHECK (iIdAsignacion > 0),
        CONSTRAINT CK_tblAsignaciones_Fechas CHECK (dFechaFin >= dFechaInicio),
        CONSTRAINT FK_tblAsignaciones_tblMaestros FOREIGN KEY (iNumeroEmpleado) REFERENCES dbo.tblMaestros (iNumeroEmpleado),
        CONSTRAINT FK_tblAsignaciones_tblMaterias FOREIGN KEY (iIdMateria) REFERENCES dbo.tblMaterias (iIdMateria)
    );
END
GO

/* ---------- tblUnidadDepartamento (unidades <-> departamentos, muchos a muchos) ----------
   Va DESPUES de tblUnidades y tblDepartamentos.
   Los nombres de las restricciones son los que tiene la base real. */
IF OBJECT_ID(N'dbo.tblUnidadDepartamento', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblUnidadDepartamento
    (
        iIdUnidad       INT     NOT NULL,
        iIdDepartamento TINYINT NOT NULL,
        iIdUnidadDepa   INT     NOT NULL,
        CONSTRAINT PK_UnidadDepa PRIMARY KEY (iIdUnidadDepa),
        CONSTRAINT UQ_UnidadDepa UNIQUE (iIdUnidad, iIdDepartamento),
        CONSTRAINT FK_RelacionUnidades FOREIGN KEY (iIdUnidad) REFERENCES dbo.tblUnidades (iIdUnidades),
        CONSTRAINT FK_RelacionDepartamento FOREIGN KEY (iIdDepartamento) REFERENCES dbo.tblDepartamentos (iIdDepartamento)
    );
END
GO

/* =============== 2. DATOS INICIALES =============== */

/* Usuarios para entrar al programa (cambia las contrasenas cuando quieras) */
IF NOT EXISTS (SELECT 1 FROM dbo.tblUsuarios WHERE vUsuario = 'admin')
    INSERT INTO dbo.tblUsuarios (iIdUsuario, vUsuario, vContrasena, iRol) VALUES (1, 'admin', 'admin', 2);
IF NOT EXISTS (SELECT 1 FROM dbo.tblUsuarios WHERE vUsuario = 'user')
    INSERT INTO dbo.tblUsuarios (iIdUsuario, vUsuario, vContrasena, iRol) VALUES (2, 'user', 'user', 1);
GO

/* =============== 3. CAMBIOS A TABLAS EXISTENTES (ALTER) ===============
   Aqui van, en el orden en que los hagas, los cambios a tablas que ya existen.
   Ejemplo -- NO esta activado:

   IF COL_LENGTH(N'dbo.tblCarreras', N'vSigla') IS NULL
       ALTER TABLE dbo.tblCarreras ADD vSigla VARCHAR(10) NULL;
   GO
   =================================================================== */
