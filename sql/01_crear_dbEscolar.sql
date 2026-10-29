/* =====================================================================
   dbEscolar - Script de creacion
   Control Escolar (alumnos, maestros, materias, carreras, semestres)

   - Nomenclatura tomada de las consultas que SI entraron al ultimo .exe:
       tblCarreras (iIdCarrera, vCarrera)
       tblAlumnos  (iMatricula, vNombre, vCorreo, vTelefono, iIdCarrera)
       tblMaestros (iNumeroEmpleado, vNombre, vCorreo, vTelefono, iIdCarrera)
       tblMaterias (iIdMateria, vNombre)
       tblSemestres(iIdSemestre, iValorSemestre)
   - Es re-ejecutable: no borra ni duplica nada si ya existe.
   - Los tipos de dato y restricciones son una propuesta (el script original
     de la base de datos no venia en el proyecto).
   ===================================================================== */

IF DB_ID(N'dbEscolar') IS NULL
    CREATE DATABASE dbEscolar;
GO

USE dbEscolar;
GO

/* ---------- tblCarreras ---------- */
IF OBJECT_ID(N'dbo.tblCarreras', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblCarreras
    (
        iIdCarrera INT          NOT NULL,
        vCarrera   VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblCarreras PRIMARY KEY (iIdCarrera),
        CONSTRAINT CK_tblCarreras_iIdCarrera CHECK (iIdCarrera > 0)
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
        vNombre    VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblMaterias PRIMARY KEY (iIdMateria),
        CONSTRAINT CK_tblMaterias_iIdMateria CHECK (iIdMateria > 0)
    );
END
GO

/* ---------- tblSemestres (la clase clsSemestre si entro al .exe; la ventana aun esta vacia) ---------- */
IF OBJECT_ID(N'dbo.tblSemestres', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblSemestres
    (
        iIdSemestre    INT NOT NULL,
        iValorSemestre INT NOT NULL,
        CONSTRAINT PK_tblSemestres PRIMARY KEY (iIdSemestre),
        CONSTRAINT CK_tblSemestres_iIdSemestre CHECK (iIdSemestre > 0)
    );
END
GO
