# citas

Registra las citas agendadas: cada cita es de una `Mascota` atendida por un `Veterinario`. Es la única entidad "N" de dos relaciones 1:N distintas a la vez — `MascotaId` y `VeterinarioId` son dos FK independientes en la misma fila, no una relación compuesta.

**Files:** `Controllers/CitasController.cs`, `Models/Cita.cs`.

## Entidades / Tablas

| Entidad | Tabla | Notas |
|---|---|---|
| `Cita` | `Citas` | Dos FK obligatorias: `MascotaId` → `Mascotas.Id`, `VeterinarioId` → `Veterinarios.Id`. Sin unique constraint en BD sobre `(VeterinarioId, FechaHora)` — la regla de "horario ocupado" vive solo en el controlador (ver `flujo.md`). |

## Endpoints

| Metodo | Ruta | Input | Output |
|---|---|---|---|
| GET | `/api/citas?mascotaId=&veterinarioId=` | query string opcional | `200` lista filtrada |
| GET | `/api/citas/{id}` | — | `200` / `404` |
| GET | `/api/mascotas/{mascotaId}/citas` | — (ruta anidada) | `200` / `404` si la mascota no existe |
| GET | `/api/veterinarios/{veterinarioId}/citas` | — (ruta anidada) | `200` / `404` si el veterinario no existe |
| POST | `/api/citas` | `Cita` (sin `Id`) | `201` + `Location` / `400` validacion o FK inexistente / `409` horario ocupado |
| PUT | `/api/citas/{id}` | `Cita` completa | `204` / `400` id no coincide o FK inexistente / `404` / `409` horario ocupado |
| DELETE | `/api/citas/{id}` | — | `204` / `404` |
