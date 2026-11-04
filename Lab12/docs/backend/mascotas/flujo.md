# Flujo — mascotas

## GET /api/mascotas
`_context.Mascotas.Include(m => m.Propietario).AsNoTracking().AsQueryable()`, luego:
- si `especie` viene en la query string (no vacio) → `.Where(m => m.Especie == especie)` (comparacion exacta, sensible a como llego el dato: "Perro" no matchea "perro").
- si `propietarioId` viene → `.Where(m => m.PropietarioId == propietarioId.Value)`.

Ambos filtros son opcionales y combinables (AND). `ToListAsync()` → `Ok(mascotas)`. Cada mascota devuelta trae su `Propietario` embebido (por el `Include`).

## GET /api/mascotas/{id}
`Include(Propietario)` + `FirstOrDefaultAsync(m => m.Id == id)`. `null` → `404`; si no → `200` con el propietario embebido.

## GET /api/propietarios/{propietarioId}/mascotas (ruta anidada)
Vive en este controlador (no en `PropietariosController`) porque devuelve mascotas, solo que filtradas por propietario. Usa `[HttpGet("/api/propietarios/{propietarioId}/mascotas")]` con ruta **absoluta** para no heredar el prefijo `api/mascotas` del controlador.

1. `_context.Propietarios.AsNoTracking().AnyAsync(p => p.Id == propietarioId)` — si el propietario no existe → `NotFound(...)` (`404`). Esto es antes que nada: el recurso padre de la ruta debe existir.
2. Si existe: `_context.Mascotas.AsNoTracking().Where(m => m.PropietarioId == propietarioId).ToListAsync()` → `Ok(mascotas)`. **Sin** `Include(Propietario)` aqui (seria redundante, ya se sabe cual es el propietario por la ruta).

## POST /api/mascotas
1. `_context.Propietarios.AnyAsync(p => p.Id == mascota.PropietarioId)` — si el `PropietarioId` enviado no existe → `BadRequest(...)` (`400`), no `404`: el recurso de la ruta (`/api/mascotas`) si existe, el dato invalido esta en el cuerpo.
2. Si existe: `Add` + `SaveChangesAsync()` → `CreatedAtAction(nameof(GetMascota), new { id = mascota.Id }, mascota)` (`201`).

El body solo necesita `PropietarioId` (no un objeto `Propietario` completo) porque la navegacion `Mascota.Propietario` es nullable (ver `validacion.md`).

## PUT /api/mascotas/{id}
1. `id != mascota.Id` → `BadRequest(...)` (`400`).
2. `AnyAsync(m => m.Id == id)` — no existe → `NotFound()` (`404`).
3. `AnyAsync(p => p.Id == mascota.PropietarioId)` sobre `Propietarios` — FK invalida → `BadRequest(...)` (`400`).
4. `Entry(mascota).State = EntityState.Modified` + `SaveChangesAsync()` → `NoContent()` (`204`).

## DELETE /api/mascotas/{id}
1. `FindAsync(id)` → `404` si `null`.
2. `_context.Citas.AnyAsync(c => c.MascotaId == id)` — si la mascota tiene al menos una cita → `Conflict(...)` (`409`), **no se elimina**.
3. Si no tiene citas: `Remove` + `SaveChangesAsync()` → `NoContent()` (`204`).

Este chequeo se agregó cuando se introdujo `Cita` (`Controllers/MascotasController.cs`): antes `Mascota` no tenía entidades dependientes, así que `DeleteMascota` borraba directo. Sin el chequeo, intentar borrar una mascota con citas haría que `SaveChangesAsync()` lance una `DbUpdateException` sin manejar (la FK `FK_Citas_Mascotas` no tiene `ON DELETE CASCADE`) — un `500`, no el `409` limpio que da ahora.
