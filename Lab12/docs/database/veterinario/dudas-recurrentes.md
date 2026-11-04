# Dudas recurrentes — Veterinario

**P: ¿Por qué `Licencia` es `VARCHAR` y no `NVARCHAR` como los demás campos de texto?**
R: Una licencia profesional (formato `CMVP0001`) solo usa caracteres ASCII (letras mayúsculas + dígitos), así que no hace falta `NVARCHAR` (Unicode). El modelo C# lo confirma con `[Unicode(false)]` (`Database/create_veterinaria.sql`, `Models/Veterinario.cs`).

**P: Si `Licencia` ya es `UNIQUE` en la base, ¿por qué el controlador vuelve a chequear duplicados con una consulta propia?**
R: Mismo motivo que `Dni` en `Propietario`: para devolver un `409 Conflict` legible en vez de dejar que la constraint rechace el `INSERT`/`UPDATE` con una `DbUpdateException` sin manejar (ver `docs/database/propietario/dudas-recurrentes.md`).
