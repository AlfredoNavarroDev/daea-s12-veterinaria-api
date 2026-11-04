# Flujo — veterinarios

Mismo patrón que `propietarios` (ver [docs/backend/propietarios/flujo.md](../propietarios/flujo.md)), aplicado a `Licencia` en vez de `Dni` y a `Citas` en vez de `Mascotas`.

## GET /api/veterinarios
`_context.Veterinarios.AsNoTracking().ToListAsync()` → `Ok(veterinarios)`. Sin paginación ni filtro.

## GET /api/veterinarios/{id}
`FirstOrDefaultAsync(v => v.Id == id)` con `AsNoTracking()`. `null` → `404`; si no → `200`.

## POST /api/veterinarios
1. `AnyAsync(v => v.Licencia == veterinario.Licencia)` — licencia ya usada por otro veterinario → `Conflict(...)` (`409`).
2. Si no hay duplicado: `Add` + `SaveChangesAsync()` → `CreatedAtAction(nameof(GetVeterinario), ...)` (`201`).

El `[ApiController]` ya rechaza con `400` antes de llegar aquí si falta `Nombres`/`Apellidos`/`Licencia`, o si `Licencia` no matchea `^[A-Z]{4}\d{4}$` (ver `validacion.md`).

## PUT /api/veterinarios/{id}
1. `id != veterinario.Id` → `BadRequest(...)` (`400`).
2. `AnyAsync(v => v.Id == id)` — no existe → `NotFound()` (`404`).
3. `AnyAsync(v => v.Licencia == veterinario.Licencia && v.Id != id)` — licencia usada por *otro* veterinario → `Conflict(...)` (`409`).
4. `Entry(veterinario).State = EntityState.Modified` + `SaveChangesAsync()` → `NoContent()` (`204`).

## DELETE /api/veterinarios/{id}
1. `FindAsync(id)` — `null` → `NotFound()` (`404`).
2. `_context.Citas.AnyAsync(c => c.VeterinarioId == id)` — si tiene al menos una cita → `Conflict(...)` (`409`), **no se elimina**.
3. Si no tiene citas: `Remove` + `SaveChangesAsync()` → `NoContent()` (`204`).

Igual que con `Propietario`/`Mascota` (ver `docs/database/veterinario/`), la FK `FK_Citas_Veterinarios` tampoco tiene `ON DELETE CASCADE` a nivel de BD — este chequeo evita además una excepción de FK sin manejar si se intentara borrar de todas formas.
