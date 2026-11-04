# veterinarios

Gestiona al plantel de veterinarios de la clínica. Es el lado "1" de una de las dos relaciones 1:N que convergen en `Cita` (la otra es `Mascota`, ver [citas](../citas/)).

**Files:** `Controllers/VeterinariosController.cs`, `Models/Veterinario.cs`.

## Entidades / Tablas

| Entidad | Tabla | Notas |
|---|---|---|
| `Veterinario` | `Veterinarios` | `Licencia` tiene constraint `UNIQUE` (`UQ_Veterinarios_Licencia`) a nivel de BD, además de la validación de formato en el modelo. |

## Endpoints

| Metodo | Ruta | Input | Output |
|---|---|---|---|
| GET | `/api/veterinarios` | — | `200` lista completa |
| GET | `/api/veterinarios/{id}` | — | `200` / `404` |
| POST | `/api/veterinarios` | `Veterinario` (sin `Id`) | `201` + `Location` / `400` validacion / `409` licencia duplicada |
| PUT | `/api/veterinarios/{id}` | `Veterinario` completo | `204` / `400` id no coincide / `404` / `409` licencia duplicada |
| DELETE | `/api/veterinarios/{id}` | — | `204` / `404` / `409` tiene citas |
