# Flujo — propietarios

## GET /api/propietarios
`_context.Propietarios.AsNoTracking().ToListAsync()` → `Ok(propietarios)`. Sin paginacion ni filtro.

## GET /api/propietarios/{id}
`FirstOrDefaultAsync(p => p.Id == id)` con `AsNoTracking()`. Si es `null` → `NotFound()` (`404`); si no → `Ok(propietario)`.

## POST /api/propietarios
1. `AnyAsync(p => p.Dni == propietario.Dni)` — si ya existe un propietario con ese DNI → `Conflict(...)` (`409`), con mensaje indicando el DNI repetido.
2. Si no hay duplicado: `Add` + `SaveChangesAsync()` → `CreatedAtAction(nameof(GetPropietario), new { id = propietario.Id }, propietario)` (`201`, con header `Location` apuntando al nuevo recurso).

El `[ApiController]` ya rechaza con `400` antes de llegar aqui si falta `Nombres`, `Apellidos` o `Dni`, o si `Dni` no son 8 digitos (ver `validacion.md`).

## PUT /api/propietarios/{id}
1. `id != propietario.Id` → `BadRequest(...)` (`400`). El id de la ruta manda; si el cuerpo trae otro id, se rechaza antes de tocar la BD.
2. `AnyAsync(p => p.Id == id)` — si no existe → `NotFound()` (`404`).
3. `AnyAsync(p => p.Dni == propietario.Dni && p.Id != id)` — DNI usado por *otro* propietario → `Conflict(...)` (`409`).
4. `_context.Entry(propietario).State = EntityState.Modified` + `SaveChangesAsync()` → `NoContent()` (`204`).

## DELETE /api/propietarios/{id}
1. `FindAsync(id)` — si es `null` → `NotFound()` (`404`).
2. `_context.Mascotas.AnyAsync(m => m.PropietarioId == id)` — si tiene al menos una mascota → `Conflict(...)` (`409`), **no se elimina**.
3. Si no tiene mascotas: `Remove` + `SaveChangesAsync()` → `NoContent()` (`204`).

Esta es la regla de negocio "no eliminar un registro con dependientes" (`Controllers/PropietariosController.cs`, metodo `DeletePropietario`). A nivel de BD la FK tampoco tiene `ON DELETE CASCADE` (ver `docs/database/propietario/`), asi que aunque este chequeo se salte, SQL Server rechazaria el `DELETE` igual — pero con un error generico en vez del `409` limpio que da la API.
