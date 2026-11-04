# Veterinario

Representa a un veterinario de la clínica. Es el lado "1" de una de las dos relaciones que convergen en `Cita` (la otra es `Mascota`, ver [cita](../cita/)).

**Files:** `Models/Veterinario.cs`, `Database/create_veterinaria.sql`, `Data/VeterinariaContext.cs`.

## Columns / fields

| Nombre | Tipo (BD) | Constraints/default | Computed/generated |
|---|---|---|---|
| `Id` | `INT IDENTITY(1,1)` | `PRIMARY KEY` | Sí — autoincremental |
| `Nombres` | `NVARCHAR(80)` | `NOT NULL` | No |
| `Apellidos` | `NVARCHAR(80)` | `NOT NULL` | No |
| `Licencia` | `VARCHAR(10)` | `NOT NULL`, `UNIQUE` (`UQ_Veterinarios_Licencia`) | No |
| `Especialidad` | `NVARCHAR(60)` | `NULL` | No |
| `Telefono` | `NVARCHAR(9)` | `NULL` | No |

## Indexes and relations

- `PRIMARY KEY` sobre `Id` (nombre autogenerado por SQL Server).
- `UNIQUE` sobre `Licencia` (`UQ_Veterinarios_Licencia`), nombrado explícitamente en el script. Es la base de la regla "licencia duplicada" en `VeterinariosController`, aplicada igual que `UQ_Propietarios_Dni`: el controlador la chequea con `AnyAsync` propio, no deja que la BD rechace el `INSERT`.
- Referenciado por `Citas.VeterinarioId` (`FK_Citas_Veterinarios`). Sin `ON DELETE CASCADE`/`SET NULL` en el script → `NO ACTION` por defecto. `VeterinariosController.DeleteVeterinario` bloquea el borrado con `409` antes de que esto se llegue a probar.
