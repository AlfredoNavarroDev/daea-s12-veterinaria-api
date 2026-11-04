# Cita

Representa una cita agendada entre una `Mascota` y un `Veterinario`. Es la única tabla del esquema con dos claves foráneas obligatorias hacia dos tablas "padre" distintas.

**Files:** `Models/Cita.cs`, `Database/create_veterinaria.sql`, `Data/VeterinariaContext.cs`.

## Columns / fields

| Nombre | Tipo (BD) | Constraints/default | Computed/generated |
|---|---|---|---|
| `Id` | `INT IDENTITY(1,1)` | `PRIMARY KEY` | Sí — autoincremental |
| `FechaHora` | `DATETIME2` | `NOT NULL` | No — mapeada a `DateTime` en C# |
| `Motivo` | `NVARCHAR(200)` | `NOT NULL` | No |
| `MascotaId` | `INT` | `NOT NULL`, `FOREIGN KEY` → `Mascotas.Id` | No |
| `VeterinarioId` | `INT` | `NOT NULL`, `FOREIGN KEY` → `Veterinarios.Id` | No |

## Indexes and relations

- `PRIMARY KEY` sobre `Id` (nombre autogenerado por SQL Server).
- `FOREIGN KEY` `FK_Citas_Mascotas` sobre `MascotaId` → `Mascotas.Id`. Sin `ON DELETE`/`ON UPDATE` explícito → `NO ACTION` por defecto.
- `FOREIGN KEY` `FK_Citas_Veterinarios` sobre `VeterinarioId` → `Veterinarios.Id`. Mismo comportamiento por defecto.
- **No** hay ningún índice `UNIQUE` sobre `(VeterinarioId, FechaHora)` a nivel de BD — la regla de "horario ocupado" (ver `docs/backend/citas/flujo.md`) se aplica únicamente en el controlador con una consulta `AnyAsync`, no está respaldada por una constraint. Si alguien insertara dos citas con el mismo veterinario y la misma fecha/hora directamente por SQL, la base lo permitiría sin quejarse.
- En el modelo C#, `Cita.Mascota` y `Cita.Veterinario` son **nullable** (`Mascota?`, `Veterinario?`) a pesar de que ambas columnas son `NOT NULL` en la BD — decisión de la capa de aplicación (ver `docs/backend/citas/dudas-recurrentes.md`), no refleja la constraint real.
