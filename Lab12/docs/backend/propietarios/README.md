# propietarios

Gestiona a los clientes de la veterinaria (propietarios de mascotas). Es el lado "1" de la relacion 1:N con `Mascota`.

**Files:** `Controllers/PropietariosController.cs`, `Models/Propietario.cs`.

## Entidades / Tablas

| Entidad | Tabla | Notas |
|---|---|---|
| `Propietario` | `Propietarios` | `Dni` tiene constraint `UNIQUE` (`UQ_Propietarios_Dni`) a nivel de BD, ademas de la validacion de formato en el modelo. |

## Endpoints

| Metodo | Ruta | Input | Output |
|---|---|---|---|
| GET | `/api/propietarios` | — | `200` lista completa |
| GET | `/api/propietarios/{id}` | — | `200` / `404` |
| POST | `/api/propietarios` | `Propietario` (sin `Id`) | `201` + `Location` / `400` validacion / `409` DNI duplicado |
| PUT | `/api/propietarios/{id}` | `Propietario` completo | `204` / `400` id no coincide / `404` / `409` DNI duplicado |
| DELETE | `/api/propietarios/{id}` | — | `204` / `404` / `409` tiene mascotas |
