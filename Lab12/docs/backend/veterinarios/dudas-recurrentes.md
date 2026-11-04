# Dudas recurrentes — veterinarios

**P: ¿Por qué `Licencia` exige el formato exacto `CMVP0001` (4 letras + 4 dígitos) en vez de aceptar cualquier texto?**
R: Es una decisión de diseño análoga al DNI de `Propietario`: la columna (`VARCHAR(10)`) aceptaría cualquier cosa, pero el `[RegularExpression(@"^[A-Z]{4}\d{4}$")]` del modelo fuerza el formato real de una licencia/colegiatura profesional (`Models/Veterinario.cs`).

**P: ¿Qué pasa si se intenta borrar un veterinario que tiene citas agendadas?**
R: `409 Conflict`, sin tocar la base — `DeleteVeterinario` hace `_context.Citas.AnyAsync(c => c.VeterinarioId == id)` antes del `Remove` (`Controllers/VeterinariosController.cs`). Mismo patrón que `Propietario` con `Mascotas` y que `Mascota` con `Citas`.

**P: ¿Existe alguna relación entre `VeterinariosController` y `MascotasController`?**
R: No directamente. Ambos son el lado "1" de una relación hacia `Cita` (que tiene las dos FK, `MascotaId` y `VeterinarioId`), pero ninguno de los dos controladores consulta al otro — solo consultan `_context.Citas` (ver [citas](../citas/)).
