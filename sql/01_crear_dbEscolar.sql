/* =====================================================================
   dbEscolar - Script de creacion  (proyecto projMenuGeneral)

   COMO USAR ESTE ARCHIVO
   - Se puede ejecutar completo las veces que quieras: cada bloque pregunta
     primero si el objeto ya existe y, si existe, no lo toca.
   - Para AGREGAR UNA TABLA NUEVA: copia un bloque "IF OBJECT_ID ... CREATE TABLE"
     al final de la seccion 1. Si la tabla tiene llave foranea, va DESPUES de la
     tabla que referencia.
   - Para CAMBIAR UNA TABLA QUE YA EXISTE: no edites su CREATE TABLE (no se
     ejecutaria en una base que ya la tiene). Agrega un bloque en la seccion 2:
         IF COL_LENGTH(N'dbo.tblX', N'columna') IS NULL
             ALTER TABLE dbo.tblX ADD columna TIPO NULL;
         <-- aqui va un GO, solo en su propia linea
   - Los GO separan lotes: despues de crear/alterar algo, lo que use ese cambio
     va en el lote siguiente.

   NOMENCLATURA (tomada de tus clases):
       tblCarreras  (iIdCarrera, vCarrera)
       tblAlumnos   (iMatricula, vNombre, vCorreo, vTelefono, iIdCarrera)
       tblMaestros  (iNumeroEmpleado, vNombre, vCorreo, vTelefono, iIdCarrera)
       tblMaterias  (iIdMateria, vNombre)
       tblSemestres (iIdSemestre, iValorSemestre)
       tblTurnos    (iIdTurnos, iValorTurnos)          <- clsTurnos NO entro al ultimo .exe
       tblUnidades  (iIdUnidades, iValorUnidades)      <- clsUnidades NO entro al ultimo .exe

   Los tipos de dato son una propuesta: el script original de la base no venia
   en el proyecto.
   ===================================================================== */

IF DB_ID(N'dbEscolar') IS NULL
    CREATE DATABASE dbEscolar;
GO

USE dbEscolar;
GO

/* =============== 1. TABLAS =============== */

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

/* ---------- tblTurnos (clsTurnos: todavia no entro al .exe; el formulario esta sin terminar) ----------
   Ojo: tu clase guarda un TEXTO ('{_nombre}') en iValorTurnos, por eso es VARCHAR aunque el
   nombre lleve prefijo "i". Si prefieres numero, o renombrar la columna, hay que cambiar tambien clsTurnos. */
IF OBJECT_ID(N'dbo.tblTurnos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblTurnos
    (
        iIdTurnos    INT         NOT NULL,
        iValorTurnos VARCHAR(50) NOT NULL,
        CONSTRAINT PK_tblTurnos PRIMARY KEY (iIdTurnos),
        CONSTRAINT CK_tblTurnos_iIdTurnos CHECK (iIdTurnos > 0)
    );
END
GO

/* ---------- tblUnidades (clsUnidades: todavia no entro al .exe; el formulario esta sin terminar) ----------
   clsUnidades recibe un municipio pero no lo guarda; cuando lo agregues va como columna nueva en la seccion 2. */
IF OBJECT_ID(N'dbo.tblUnidades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblUnidades
    (
        iIdUnidades    INT          NOT NULL,
        iValorUnidades VARCHAR(100) NOT NULL,
        CONSTRAINT PK_tblUnidades PRIMARY KEY (iIdUnidades),
        CONSTRAINT CK_tblUnidades_iIdUnidades CHECK (iIdUnidades > 0)
    );
END
GO


/* =============== 2. CAMBIOS A TABLAS EXISTENTES (ALTER) ===============
   Aqui van, en el orden en que los hiciste, los cambios que ya probaste y quieres conservar.
   Ejemplo (Departamentos, de tu Cambios.md) -- NO esta activado:

   IF OBJECT_ID(N'dbo.tblDepartamentos', N'U') IS NULL
       CREATE TABLE dbo.tblDepartamentos (iIdDepartamento TINYINT NOT NULL PRIMARY KEY, vNombre VARCHAR(100) NOT NULL);
   <-- aqui va un GO, solo en su propia linea
   IF COL_LENGTH(N'dbo.tblCarreras', N'iIdDepartamento') IS NULL
       ALTER TABLE dbo.tblCarreras ADD iIdDepartamento TINYINT NULL;
   <-- aqui va un GO, solo en su propia linea
   =================================================================== */
