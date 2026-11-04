# Dudas recurrentes — citas

**P: La regla de "horario ocupado", ¿evita que dos citas se superpongan, o solo que coincidan exactamente?**
R: Solo que coincidan exactamente. `PostCita`/`PutCita` comparan `c.FechaHora == cita.FechaHora` — una igualdad de `DateTime`, no un rango de duración. Dos citas del mismo veterinario a las `09:00:00` y `09:15:00` no chocan aunque una dure una hora (`Controllers/CitasController.cs`). El modelo no tiene un campo de duración, así que no habría con qué calcular el rango.

**P: ¿Por qué `Cita.Mascota` y `Cita.Veterinario` son nullable si ambas FK son obligatorias (`NOT NULL` en BD)?**
R: Mismo motivo que `Mascota.Propietario` (ver `docs/backend/mascotas/dudas-recurrentes.md`): para que el `POST`/`PUT` solo necesiten enviar `MascotaId`/`VeterinarioId`, sin los objetos completos. La columna sigue siendo obligatoria; solo la navegación en memoria puede quedar sin cargar.

**P: ¿Por qué las rutas anidadas (`/api/mascotas/{id}/citas` y `/api/veterinarios/{id}/citas`) viven las dos en `CitasController` y no una en cada controlador padre?**
R: Porque ambas devuelven objetos `Cita` (mismo tipo que el resto de este controlador), solo que filtrados por un lado distinto de la relación. Cada una usa `[HttpGet]` con ruta absoluta para no quedar bajo el prefijo `api/citas` (`Controllers/CitasController.cs` → `GetCitasPorMascota`, `GetCitasPorVeterinario`), igual que `GetMascotasPorPropietario` en `MascotasController`.

**P: ¿`Cita` valida que `FechaHora` no sea una fecha ya pasada?**
R: No. `[Required]` solo exige que el campo no sea nulo; no hay ninguna regla (Data Annotation ni chequeo en el controlador) que rechace una fecha anterior a hoy.
