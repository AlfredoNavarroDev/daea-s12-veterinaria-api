# Validación — mascotas

| Campo | Reglas |
|---|---|
| `Nombre` | `[Required]`, `[StringLength(60)]` |
| `Especie` | `[Required]`, `[StringLength(40)]` |
| `Raza` | `[StringLength(60)]`, nullable |
| `FechaNacimiento` | `DateOnly?`, sin restriccion adicional (cualquier fecha, pasada o futura, es valida) |
| `PropietarioId` | `[Required]` (int no-nulo; el atributo es redundante para un value type pero se dejo explicito) |
| `Propietario` | nav **nullable** (`Propietario?`) — a proposito, no es un olvido: ver `dudas-recurrentes.md` |

La validez *referencial* de `PropietarioId` (que el id apunte a un propietario real) no la cubre ninguna Data Annotation — se resuelve en el controlador con `AnyAsync` contra `Propietarios` (ver `flujo.md`), y responde `400`, no una excepcion de FK sin manejar.
