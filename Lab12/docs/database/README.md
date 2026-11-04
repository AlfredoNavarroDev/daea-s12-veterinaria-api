# Database

SQL Server, enfoque **Database First**: el esquema real nace en `Database/create_veterinaria.sql`; el modelo C# (`Models/*.cs`, `Data/VeterinariaContext.cs`) se generó desde ahí con `dotnet ef dbcontext scaffold --data-annotations` y luego se ajustó a mano (ver `docs/backend/README.md`). El script SQL es la fuente de verdad del esquema; el modelo es lo que la aplicación *cree* que es ese esquema.

`Database/create_veterinaria_cloud.sql` es el mismo esquema y los mismos datos de ejemplo, sin las instrucciones `CREATE DATABASE`/`USE` — pensado para correr contra una base ya provisionada por el hosting (MonsterASP.NET), donde esas instrucciones no aplican o no están permitidas.

Collation de la base: `Modern_Spanish_CI_AS` (case-insensitive, accent-sensitive) — afecta cualquier comparación de texto hecha vía LINQ (ver `docs/backend/mascotas/dudas-recurrentes.md`).

## Unidades

| Unidad | Tabla | Descripcion |
|---|---|---|
| [propietario](./propietario/) | `Propietarios` | Clientes de la veterinaria |
| [mascota](./mascota/) | `Mascotas` | Mascotas, cada una con un `Propietario` obligatorio |

## Known gaps

- Ninguna migración de EF Core versiona este esquema (no existe carpeta `Migrations/`): el script SQL se corre manualmente y el DbContext se re-scaffoldea si el esquema cambia. Un cambio de esquema hecho solo en el script sin re-scaffoldear deja el modelo C# desincronizado sin que nada lo avise en compilación.
