# Dudas recurrentes — mascotas

**P: ¿Por qué `Mascota.Propietario` es `Propietario?` (nullable) si `PropietarioId` es obligatorio y la relación es 1:N real?**
R: Para que el `POST`/`PUT` puedan enviar solo `PropietarioId` en el cuerpo, sin tener que incluir el objeto `Propietario` completo. Si la propiedad de navegación fuera no-nula, `[ApiController]` la trataría como requerida en el body y un `POST` con solo `propietarioId` fallaría con `400` por falta del campo `propietario` (`Models/Mascota.cs`, `Controllers/MascotasController.cs` → `PostMascota`).

**P: ¿Por qué crear una mascota con un `propietarioId` que no existe devuelve `400` y no `404`?**
R: `404` se reserva para "el recurso de la ruta no existe". En un `POST /api/mascotas` el recurso de la ruta es la colección de mascotas, que sí existe — el dato inválido está en el cuerpo de la petición, por eso es `400` (`Controllers/MascotasController.cs` → `PostMascota`, `PutMascota`).

**P: ¿Por qué `GET /api/propietarios/{propietarioId}/mascotas` está en `MascotasController` y no en `PropietariosController`?**
R: Porque lo que devuelve son objetos `Mascota` (mismo tipo que el resto del controlador), solo que filtrados por propietario; la ruta se declara con `[HttpGet("/api/propietarios/{propietarioId}/mascotas")]` usando una ruta absoluta para no quedar bajo el prefijo `api/mascotas` del controlador (`Controllers/MascotasController.cs` → `GetMascotasPorPropietario`).

**P: El filtro `especie` en `GET /api/mascotas?especie=Perro`, ¿es case-insensitive?**
R: Sí, pero no por el código C# — es `.Where(m => m.Especie == especie)` traducido a SQL, y la collation de `VeterinariaDB` es `Modern_Spanish_CI_AS` (CI = case-insensitive), así que "perro" en minúsculas sí matchea "Perro" en la base. Si se cambiara la collation a `_CS` (case-sensitive), este mismo filtro dejaría de matchear sin tocar el controlador (`Controllers/MascotasController.cs` → `GetMascotas`).
