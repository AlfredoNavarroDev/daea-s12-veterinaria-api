# Flujo — citas

## GET /api/citas
`_context.Citas.Include(c => c.Mascota).Include(c => c.Veterinario).AsNoTracking().AsQueryable()`, luego:
- si `mascotaId` viene en la query string → `.Where(c => c.MascotaId == mascotaId.Value)`.
- si `veterinarioId` viene → `.Where(c => c.VeterinarioId == veterinarioId.Value)`.

Ambos combinables (AND). `ToListAsync()` → `Ok(citas)`. Cada cita trae `Mascota` y `Veterinario` embebidos (dos `Include` separados); `Mascota.Propietario` dentro de ese JSON queda `null` porque no hay un `ThenInclude(m => m.Propietario)` — no es un bug, simplemente no se cargó esa relación de segundo nivel.

## GET /api/citas/{id}
Mismo `Include` doble + `FirstOrDefaultAsync(c => c.Id == id)`. `null` → `404`; si no → `200`.

## GET /api/mascotas/{mascotaId}/citas (ruta anidada)
Vive en `CitasController`, no en `MascotasController`, con `[HttpGet("/api/mascotas/{mascotaId}/citas")]` (ruta absoluta).
1. `_context.Mascotas.AnyAsync(m => m.Id == mascotaId)` — mascota no existe → `NotFound(...)` (`404`).
2. Si existe: `_context.Citas.Where(c => c.MascotaId == mascotaId).ToListAsync()` → `Ok(citas)`, **sin** `Include` (seria redundante, la mascota ya se conoce por la ruta).

## GET /api/veterinarios/{veterinarioId}/citas (ruta anidada)
Mismo patrón que la anterior, pero filtrando por `VeterinarioId` y verificando que el veterinario exista (`[HttpGet("/api/veterinarios/{veterinarioId}/citas")]`).

## POST /api/citas
1. `_context.Mascotas.AnyAsync(m => m.Id == cita.MascotaId)` — no existe → `BadRequest(...)` (`400`).
2. `_context.Veterinarios.AnyAsync(v => v.Id == cita.VeterinarioId)` — no existe → `BadRequest(...)` (`400`).
3. `_context.Citas.AnyAsync(c => c.VeterinarioId == cita.VeterinarioId && c.FechaHora == cita.FechaHora)` — el veterinario ya tiene otra cita exactamente en esa `FechaHora` → `Conflict(...)` (`409`). Es una comparación de igualdad exacta de `DateTime`, no un rango: dos citas a las 09:00:00 y 09:00:01 no chocan.
4. Si pasa las tres: `Add` + `SaveChangesAsync()` → `CreatedAtAction(nameof(GetCita), ...)` (`201`).

El body solo necesita `MascotaId` y `VeterinarioId` (no los objetos completos) porque `Cita.Mascota` y `Cita.Veterinario` son navegaciones nullable (ver `validacion.md`).

## PUT /api/citas/{id}
1. `id != cita.Id` → `BadRequest(...)` (`400`).
2. `AnyAsync(c => c.Id == id)` — no existe → `NotFound()` (`404`).
3. FK de `Mascota` y `Veterinario` validadas igual que en `POST` → `400` si alguna no existe.
4. `AnyAsync(c => c.VeterinarioId == ... && c.FechaHora == ... && c.Id != id)` — mismo chequeo de horario ocupado, excluyendo la propia cita → `409`.
5. `Entry(cita).State = EntityState.Modified` + `SaveChangesAsync()` → `NoContent()` (`204`).

## DELETE /api/citas/{id}
`FindAsync(id)` → `404` si `null`; si no, `Remove` + `SaveChangesAsync()` → `NoContent()` (`204`). `Cita` no tiene entidades dependientes, así que no hay chequeo de negocio adicional aquí (a diferencia de `Mascota` y `Veterinario`, que sí lo tienen por ser el lado "1" de esta misma relación).
