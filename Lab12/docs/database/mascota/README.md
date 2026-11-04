# Mascota

Representa una mascota atendida por la veterinaria. Toda mascota pertenece a exactamente un `Propietario`.

**Files:** `Models/Mascota.cs`, `Database/create_veterinaria.sql`, `Data/VeterinariaContext.cs`.

## Columns / fields

| Nombre | Tipo (BD) | Constraints/default | Computed/generated |
|---|---|---|---|
| `Id` | `INT IDENTITY(1,1)` | `PRIMARY KEY` | Si — autoincremental |
| `Nombre` | `NVARCHAR(60)` | `NOT NULL` | No |
| `Especie` | `NVARCHAR(40)` | `NOT NULL` | No |
| `Raza` | `NVARCHAR(60)` | `NULL` | No |
| `FechaNacimiento` | `DATE` | `NULL` | No — mapeada a `DateOnly?` en C# |
| `PropietarioId` | `INT` | `NOT NULL`, `FOREIGN KEY` → `Propietarios.Id` | No |

## Indexes and relations

- `PRIMARY KEY` sobre `Id` (nombre autogenerado por SQL Server: `PK__Mascotas__3214EC07C97AF567`).
- `FOREIGN KEY` `FK_Mascotas_Propietarios` sobre `PropietarioId` → `Propietarios.Id`. Sin `ON DELETE`/`ON UPDATE` explícito en el script → comportamiento `NO ACTION` por defecto a nivel de BD.
- En el modelo C#, `Mascota.Propietario` es **nullable** (`Propietario?`) a pesar de que `PropietarioId` es `NOT NULL` en la BD — es una decisión de la capa de aplicación, no refleja la constraint real (ver `docs/backend/mascotas/dudas-recurrentes.md` para el porqué).
