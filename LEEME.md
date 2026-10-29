# Control Escolar (Avalonia)

Migracion de `projMenuGeneral` (WinForms, .NET Framework 4.8) a Avalonia 11 + .NET 10.

## Puesta en marcha (Fedora)

1. Crear la base de datos (con tu contenedor de SQL Server en marcha):

   ```fish
   podman cp sql/01_crear_dbEscolar.sql NOMBRE_CONTENEDOR:/tmp/
   podman exec -it NOMBRE_CONTENEDOR /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -C -i /tmp/01_crear_dbEscolar.sql
   ```

   (te pedira la contrasena; tambien puedes abrir el `.sql` en DBeaver y ejecutarlo como script).

2. Revisar `appsettings.json` (puerto y contrasena). Ese archivo NO se sube a git (ver `.gitignore`);
   `appsettings.example.json` es la plantilla sin contrasena.

3. Ejecutar:

   ```fish
   dotnet run
   ```

## Estructura

| Carpeta | Contenido |
|---|---|
| `Models/` | Clases simples: `Alumno`, `Maestro`, `Materia`, `Carrera` |
| `Data/` | `Db` (conexion + consultas con parametros) y un repositorio por entidad |
| `Services/` | `Validaciones`, `Errores`, `Dialogos` (reemplazo de MessageBox) |
| `Views/` | Ventanas `.axaml` y su codigo, mas `BarraAcciones` (menu + botones compartidos) |
| `sql/` | Script de creacion de la base de datos |

Si tu SDK no es .NET 10, cambia `TargetFramework` en `ControlEscolar.csproj`.
