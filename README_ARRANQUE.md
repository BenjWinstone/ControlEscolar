# Cómo correrlo (Fedora)

1. Con el contenedor de SQL Server encendido, ejecuta `sql/01_crear_dbEscolar.sql` (en Rider: base de datos → Run, o con `podman exec ... sqlcmd`).
2. Revisa en `Clases/clsGlobal.cs` el puerto del servidor (`localhost,1433`).
3. En Rider: abre `projMenuGeneral.slnx` (o el `.csproj`), **compila (Ctrl+F9)** y ejecuta (Shift+F10). Desde terminal: `dotnet run`.

Vista previa de las ventanas: ver el final de la respuesta en el chat (plugin AvaloniaRider, botón "Editor and Preview" y compilar primero).
