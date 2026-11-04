# Dudas recurrentes — Propietario

**P: ¿Por qué `Dni` es `CHAR(8)` y no `NVARCHAR(8)` como el resto de los campos de texto?**
R: `CHAR` de largo fijo tiene sentido porque un DNI siempre mide exactamente 8 caracteres (a diferencia de `Nombres`/`Apellidos`, que son de largo variable). El modelo refuerza "solo dígitos" aparte, con `[RegularExpression]` — la columna por sí sola aceptaría letras (`Database/create_veterinaria.sql`, `Models/Propietario.cs`).

**P: Si `Dni` ya es `UNIQUE` en la base, ¿por qué el controlador hace su propia consulta `AnyAsync` antes de insertar?**
R: Para devolver un `409 Conflict` con un mensaje legible. Si se dejara que la base rechazara el `INSERT`/`UPDATE` por la constraint `UQ_Propietarios_Dni`, EF Core lanzaría una `DbUpdateException` sin manejar — terminaría en un `500`, no en el `409` limpio que pide el enunciado (`Controllers/PropietariosController.cs`).

**P: ¿Qué pasa si se borra un Propietario con mascotas directamente por SQL (sin pasar por la API)?**
R: SQL Server lo rechaza: la FK `FK_Mascotas_Propietarios` no tiene `ON DELETE CASCADE` ni `SET NULL`, así que el comportamiento por defecto (`NO ACTION`) bloquea el `DELETE`. La API nunca llega a depender de esto porque bloquea el mismo caso antes, con su propio chequeo.
