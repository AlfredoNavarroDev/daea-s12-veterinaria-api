# Backend

API REST ASP.NET Core 10 con EF Core (Database First). No hay capa de repositorios: cada controlador inyecta `VeterinariaContext` (`Data/VeterinariaContext.cs`) directamente y hace sus propias consultas LINQ.

## Unidades

| Unidad | Archivo(s) | Descripcion |
|---|---|---|
| [propietarios](./propietarios/) | `Controllers/PropietariosController.cs`, `Models/Propietario.cs` | CRUD de propietarios, regla de DNI unico y bloqueo de borrado con mascotas dependientes |
| [mascotas](./mascotas/) | `Controllers/MascotasController.cs`, `Models/Mascota.cs` | CRUD de mascotas, filtro por query string, ruta anidada bajo `/api/propietarios/{id}/mascotas` |

## Convenciones compartidas

- `[ApiController]` en ambos controladores: una propiedad de solo-lectura no-nula sin valor en el body ya dispara `400` automatico antes de que el action corra (no hace falta `ModelState.IsValid` manual).
- Toda lectura usa `.AsNoTracking()`; toda escritura hace `SaveChangesAsync()` tras `Add`/`Remove`/marcar `EntityState.Modified`.
- Validaciones que no puede expresar una Data Annotation (DNI duplicado, FK inexistente, dependientes) se resuelven en el controlador contra el propio `DbContext`, nunca en el modelo.
- CORS: una sola politica (`ApiCorsPolicy`), origen leido de `Cors:AllowedOrigin` en configuracion (`Program.cs`), nunca `AllowAnyOrigin`.
- Swagger: documento OpenAPI generado por `Microsoft.AspNetCore.OpenApi` (`AddOpenApi()` / `MapOpenApi()`), UI servida por `Swashbuckle.AspNetCore.SwaggerUI` en `/swagger`, visible en todos los entornos (no solo Development).

## Known gaps

- No hay archivos de test automatizados (xUnit/NUnit) en el proyecto; la unica evidencia de pruebas es manual via `Lab12.http`.
