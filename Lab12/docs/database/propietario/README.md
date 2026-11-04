# Propietario

Representa a un cliente de la veterinaria, dueño de cero o más mascotas.

**Files:** `Models/Propietario.cs`, `Database/create_veterinaria.sql`, `Data/VeterinariaContext.cs`.

## Columns / fields

| Nombre | Tipo (BD) | Constraints/default | Computed/generated |
|---|---|---|---|
| `Id` | `INT IDENTITY(1,1)` | `PRIMARY KEY` | Si — autoincremental, asignado por SQL Server |
| `Nombres` | `NVARCHAR(80)` | `NOT NULL` | No |
| `Apellidos` | `NVARCHAR(80)` | `NOT NULL` | No |
| `Dni` | `CHAR(8)` | `NOT NULL`, `UNIQUE` (`UQ_Propietarios_Dni`) | No |
| `Telefono` | `NVARCHAR(9)` | `NULL` | No |
| `Email` | `NVARCHAR(120)` | `NULL` | No |
| `Direccion` | `NVARCHAR(150)` | `NULL` | No |

## Indexes and relations

- `PRIMARY KEY` sobre `Id`. El script no le da nombre explícito; SQL Server le asignó uno autogenerado (`PK__Propieta__3214EC07CDDA9A39`, visible en `VeterinariaContext.OnModelCreating`) — si se recrea la base desde cero ese nombre puede salir distinto, aunque la columna y el comportamiento sean los mismos.
- `UNIQUE` sobre `Dni` (`UQ_Propietarios_Dni`), nombrado explícitamente en el script. Es la base de la regla de negocio de "DNI duplicado" en `PropietariosController`, aunque ese controlador la aplica con una consulta `AnyAsync` propia, no dejando que la BD la rechace.
- Referenciado por `Mascotas.PropietarioId` (`FK_Mascotas_Propietarios`). El script **no** declara `ON DELETE CASCADE` ni `ON DELETE SET NULL` — el comportamiento por defecto de SQL Server es `NO ACTION`: un intento de `DELETE` sobre un `Propietario` con mascotas falla a nivel de BD. En la práctica esto nunca se llega a probar porque `PropietariosController.DeletePropietario` ya bloquea el borrado antes, con un `409` explícito.
