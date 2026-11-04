# mascotas

Gestiona las mascotas atendidas por la veterinaria. Es el lado "N" de la relacion 1:N con `Propietario`: toda mascota pertenece exactamente a un propietario (`PropietarioId` no nulo).

**Files:** `Controllers/MascotasController.cs`, `Models/Mascota.cs`.

## Entidades / Tablas

| Entidad | Tabla | Notas |
|---|---|---|
| `Mascota` | `Mascotas` | `PropietarioId` es FK obligatoria hacia `Propietarios.Id`; no tiene entidades propias dependientes. |

## Endpoints

| Metodo | Ruta | Input | Output |
|---|---|---|---|
| GET | `/api/mascotas?especie=&propietarioId=` | query string opcional | `200` lista filtrada |
| GET | `/api/mascotas/{id}` | — | `200` / `404` |
| GET | `/api/propietarios/{propietarioId}/mascotas` | — (ruta anidada) | `200` / `404` si el propietario no existe |
| POST | `/api/mascotas` | `Mascota` (sin `Id`) | `201` + `Location` / `400` validacion o FK inexistente |
| PUT | `/api/mascotas/{id}` | `Mascota` completa | `204` / `400` id no coincide o FK inexistente / `404` |
| DELETE | `/api/mascotas/{id}` | — | `204` / `404` |
