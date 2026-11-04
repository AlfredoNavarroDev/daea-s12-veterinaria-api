# Dudas recurrentes — propietarios

**P: ¿Por qué el DNI valida longitud exacta Y una expresión regular, si una ya implica a la otra en la práctica?**
R: `StringLength(8, MinimumLength = 8)` fuerza el largo; `RegularExpression(@"^\d{8}$")` fuerza que esos 8 caracteres sean dígitos. La columna de BD es `CHAR(8)`, que aceptaría letras sin problema — el regex es lo único que impide un DNI alfanumérico (`Models/Propietario.cs`).

**P: ¿Por qué el PUT devuelve 400 si el id de la ruta no coincide con el del cuerpo, en vez de simplemente usar el id de la ruta e ignorar el del cuerpo?**
R: Es una decisión explícita del controlador (`PutPropietario`, primera línea): trata la inconsistencia como una petición mal formada y la rechaza con `BadRequest`, en vez de adivinar cuál de los dos ids es el correcto.

**P: ¿Qué pasa si se intenta borrar un propietario que tiene mascotas?**
R: Devuelve `409 Conflict` sin tocar la base — `DeletePropietario` hace `_context.Mascotas.AnyAsync(m => m.PropietarioId == id)` antes del `Remove`. No hay borrado en cascada posible desde esta API.

**P: ¿El 409 por DNI duplicado se aplica también al PUT, o solo al POST?**
R: A ambos. `PostPropietario` compara contra todos los DNI existentes; `PutPropietario` hace la misma comparación pero excluyendo el propio id (`p.Id != id`), para no bloquear guardar sin cambios el propio registro.
