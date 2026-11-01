# Notas de la migración WinForms → Avalonia

Este proyecto conserva la estructura de `projMenuGeneral`: mismas carpetas (`Clases`, `Escolar`, `Operaciones`, `Figuras`, `Iconos`),
mismos nombres de clases, formularios y controles, y misma lógica (clases que devuelven el SQL, `clsConexion`, `DataSet`, validaciones en cada formulario).

Todo lo que cambió respecto a tu original está marcado en el código con `// CAMBIO` o explicado aquí.

## Cambios obligatorios (Avalonia no tiene lo que usabas en WinForms)

| Dónde | Qué cambió | Por qué |
|---|---|---|
| `*.Designer.cs` + `*.resx` → `*.axaml` | Cada formulario ahora es un `.axaml` con los **mismos nombres de controles y las mismas posiciones X, Y** (`Canvas.Left` / `Canvas.Top`) | Avalonia no usa `Designer.cs` |
| `Clases/MessageBox.cs` (nuevo) | Reemplaza `MessageBox.Show`. Se usa igual pero con `await` delante | Las ventanas emergentes de Avalonia no detienen el programa; hay que esperar a que se cierren |
| Formularios: `Buscar()`, `Guardar()`, `Eliminar()`, `ObtenerEntero`, `CampoVacio`, `Validar...` | Ahora son `async Task` (y los eventos `async void`) | Consecuencia del `await` de arriba |
| `ObtenerEntero(TextBox campo, string descripcion)` | Sin el parámetro `out int valor` | `out` no se puede usar en métodos `async`. Tu código nunca usaba ese valor (siempre hacía `int.Parse(...)` después) |
| `CampoVacio(TextBox ...)` | Antes recibía `Control` | `Control` en Avalonia no tiene `.Text` |
| `Clases/clsOpcion.cs` (nuevo) + 3 métodos en Alumnos y Maestros | Sustituyen `DataSource`, `DisplayMember`, `ValueMember` y `SelectedValue` del ComboBox | El ComboBox de Avalonia no los tiene |
| `Application.Exit()` → `Clases.clsGlobal.Salir()` | Mismo efecto (cierra todo el programa) | `Application.Exit()` es de WinForms |
| `if (!Design.IsDesignMode)` antes de `CargarCarreras()` | La vista previa de Rider ejecuta el constructor; esto evita que intente conectarse a la base | Sin esto la vista previa de Alumnos y Maestros falla |
| `System.Data.SqlClient` → `Microsoft.Data.SqlClient` | Mismas clases, solo cambia el `using` y el paquete | El paquete anterior está obsoleto |
| `clsGlobal`: `_server` y `_sec` | `localhost,1433` y `TrustServerCertificate=True` (tus valores originales quedan en comentarios) | `void\sqlserver2025` y `Integrated Security` solo existen en Windows |
| `frmAlumnos.CargarCarreras` | Usa `Clases.clsGlobal.conexion` en lugar de su cadena escrita a mano | La cadena original solo funciona en tu servidor de Windows |

## Arreglos mínimos para que compile

- `clsCarreras`: faltaba el tipo `byte` en el parámetro `idDepartamento`, y se volvió a agregar el constructor de 2 parámetros que usa `frmCarreras`.

## Lo que NO se tocó (pendientes conocidos de tu código original)

Se dejó igual a propósito. Cuando quieras, se cambia con calma:

1. **Consultas con texto concatenado** (`'{_nombre}'`): un nombre con apóstrofe (D'Angelo) rompe la consulta y permite inyección SQL.
2. **`clsConexion`** no cierra la conexión si `ExecuteNonQuery` lanza error, y siempre devuelve "Operacion Exitosa".
3. **Si la base de datos no responde, el programa se cierra** (en WinForms salía un cuadro de "excepción no controlada"; en Avalonia no existe ese cuadro).
4. **Salir** (`msSalir`) cierra el programa completo, no solo la ventana.
5. `frmMaestros.Eliminar` dice "No existe esa Matricula" (debería decir número de empleado), y Maestros no valida que haya una carrera seleccionada.
6. Alumnos, Carreras y Materias modifican un registro existente sin pedir confirmación (Maestros sí la pide).
7. La contraseña está escrita en `clsGlobal.cs`: **no subas este archivo a un repositorio público**.

## Sin terminar (igual que en tu original)

`frmSemestres`, `frmTurnos`, `frmUnidades` y `frmSuma` tienen su diseño pero ningún código. En el menú, Suma / Resta / Multiplicación / División y Figuras tienen su evento vacío.
No se migró `Properties/` (`Resources`, `Settings`, `AssemblyInfo`) ni `App.config`: son de WinForms / .NET Framework.
