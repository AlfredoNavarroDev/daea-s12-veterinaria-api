# Database

SQL Server, enfoque **Database First**: el esquema real nace en `Database/create_veterinaria.sql`; el modelo C# (`Models/*.cs`, `Data/VeterinariaContext.cs`) se generó desde ahí con `dotnet ef dbcontext scaffold --data-annotations` y luego se ajustó a mano (ver `docs/backend/README.md`). El script SQL es la fuente de verdad del esquema; el modelo es lo que la aplicación *cree* que es ese esquema.

`Database/create_veterinaria_cloud.sql` es el mismo esquema y los mismos datos de ejemplo, sin las instrucciones `CREATE DATABASE`/`USE` — pensado para correr contra una base ya provisionada por el hosting (MonsterASP.NET), donde esas instrucciones no aplican o no están permitidas.

Collation de la base: `Modern_Spanish_CI_AS` (case-insensitive, accent-sensitive) — afecta cualquier comparación de texto hecha vía LINQ (ver `docs/backend/mascotas/dudas-recurrentes.md`).

## Unidades

| Unidad | Tabla | Descripcion |
|---|---|---|
| [propietario](./propietario/) | `Propietarios` | Clientes de la veterinaria |
| [mascota](./mascota/) | `Mascotas` | Mascotas, cada una con un `Propietario` obligatorio |
| [veterinario](./veterinario/) | `Veterinarios` | Plantel de veterinarios |
| [cita](./cita/) | `Citas` | Citas, cada una con una `Mascota` y un `Veterinario` obligatorios (dos FK) |

## Known gaps

- Ninguna migración de EF Core versiona este esquema (no existe carpeta `Migrations/`): el script SQL se corre manualmente y el DbContext se re-scaffoldea (con `--force`) si el esquema cambia. Un cambio de esquema hecho solo en el script sin re-scaffoldear deja el modelo C# desincronizado sin que nada lo avise en compilación. Esto ya pasó una vez en este proyecto: al agregar `Veterinarios`/`Citas` hubo que regenerar los 4 modelos con `--force` y reaplicar a mano las Data Annotations/ajustes de los 2 modelos existentes.
- La regla de "horario ocupado" de `Cita` (ver `docs/database/cita/notas.md`) no tiene respaldo de constraint en BD — solo vive en el controlador.
