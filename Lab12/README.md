# Lab12 - API Veterinaria

Web API REST con ASP.NET Core 10 + Entity Framework Core (Database First) para el curso DAEA (TECSUP), Semana 12. Dominio: gestion de propietarios, sus mascotas, los veterinarios y las citas de una veterinaria.

## Que problema resuelve

Una veterinaria necesita registrar a sus clientes (propietarios), las mascotas que atiende (cada una de un unico propietario), su plantel de veterinarios, y las citas que agendan (cada cita es de una mascota con un veterinario). La API expone el CRUD de las cuatro entidades, filtros por query string, rutas anidadas, y valida reglas de negocio que una Data Annotation no puede expresar (duplicados, referencias inexistentes, dependientes, choques de horario).

## Entidades y relaciones

- **Propietario** (1): `Id`, `Nombres`, `Apellidos`, `Dni` (8 digitos, unico), `Telefono`, `Email`, `Direccion`.
- **Mascota** (N de Propietario): `Id`, `Nombre`, `Especie`, `Raza`, `FechaNacimiento`, `PropietarioId` (clave foranea).
- **Veterinario** (1): `Id`, `Nombres`, `Apellidos`, `Licencia` (formato `CMVP0001`, unica), `Especialidad`, `Telefono`.
- **Cita** (N de Mascota y N de Veterinario a la vez): `Id`, `FechaHora`, `Motivo`, `MascotaId`, `VeterinarioId`.
- Dos relaciones 1:N: Propietario -> Mascotas, y Mascota/Veterinario -> Citas (Cita tiene dos claves foraneas). Las propiedades de navegacion del lado "hijo" (`Mascota.Propietario`, `Cita.Mascota`, `Cita.Veterinario`) son nulas (`Tipo?`) para que el POST solo necesite enviar el id de la clave foranea. Las colecciones del lado "padre" (`Propietario.Mascotas`, `Mascota.Citas`, `Veterinario.Citas`) tienen `[JsonIgnore]` para evitar el ciclo de serializacion JSON sin usar DTOs.

## Reglas de negocio (409 Conflict)

1. **DNI duplicado**: crear o editar un Propietario con un DNI que ya existe en otro registro.
2. **Licencia duplicada**: crear o editar un Veterinario con una Licencia que ya existe en otro registro.
3. **Horario ocupado**: crear o editar una Cita con el mismo Veterinario y la misma FechaHora que otra Cita existente.
4. **Eliminar con dependientes**: eliminar un Propietario con Mascotas, una Mascota con Citas, o un Veterinario con Citas, devuelve `409` (no se permite dejar registros huerfanos).

## Estructura del proyecto

```
Lab12/
├── Controllers/
│   ├── PropietariosController.cs   CRUD de Propietarios
│   ├── MascotasController.cs       CRUD de Mascotas + filtro + ruta anidada
│   ├── VeterinariosController.cs   CRUD de Veterinarios
│   └── CitasController.cs          CRUD de Citas + filtros + 2 rutas anidadas
├── Models/
│   ├── Propietario.cs              Entidad con Data Annotations
│   ├── Mascota.cs                  Entidad con Data Annotations
│   ├── Veterinario.cs              Entidad con Data Annotations
│   └── Cita.cs                     Entidad con Data Annotations
├── Data/
│   └── VeterinariaContext.cs       DbContext (generado con Scaffold-DbContext)
├── Database/
│   ├── create_veterinaria.sql        Script local (BD primero)
│   └── create_veterinaria_cloud.sql  Mismo script, sin CREATE DATABASE/USE (hosting)
├── Program.cs                      DI del DbContext, CORS, Swagger
├── appsettings.json / appsettings.Development.json
└── README.md
```

No hay capa de repositorios: los controladores inyectan `VeterinariaContext` directamente, como pide el enunciado del laboratorio.

## Endpoints

| Metodo | Ruta | Descripcion | Codigos |
|---|---|---|---|
| GET | `/api/propietarios` | Lista todos los propietarios | 200 |
| GET | `/api/propietarios/{id}` | Detalle de un propietario | 200, 404 |
| POST | `/api/propietarios` | Crea un propietario | 201, 400, 409 |
| PUT | `/api/propietarios/{id}` | Actualiza un propietario | 204, 400, 404, 409 |
| DELETE | `/api/propietarios/{id}` | Elimina un propietario | 204, 404, 409 |
| GET | `/api/mascotas?especie=&propietarioId=` | Lista mascotas, filtrable por query string | 200 |
| GET | `/api/mascotas/{id}` | Detalle de una mascota | 200, 404 |
| GET | `/api/propietarios/{propietarioId}/mascotas` | Ruta anidada: mascotas de un propietario | 200, 404 |
| POST | `/api/mascotas` | Crea una mascota | 201, 400 |
| PUT | `/api/mascotas/{id}` | Actualiza una mascota | 204, 400, 404 |
| DELETE | `/api/mascotas/{id}` | Elimina una mascota | 204, 404, 409 |
| GET | `/api/veterinarios` | Lista todos los veterinarios | 200 |
| GET | `/api/veterinarios/{id}` | Detalle de un veterinario | 200, 404 |
| POST | `/api/veterinarios` | Crea un veterinario | 201, 400, 409 |
| PUT | `/api/veterinarios/{id}` | Actualiza un veterinario | 204, 400, 404, 409 |
| DELETE | `/api/veterinarios/{id}` | Elimina un veterinario | 204, 404, 409 |
| GET | `/api/citas?mascotaId=&veterinarioId=` | Lista citas, filtrable por query string | 200 |
| GET | `/api/citas/{id}` | Detalle de una cita | 200, 404 |
| GET | `/api/mascotas/{mascotaId}/citas` | Ruta anidada: citas de una mascota | 200, 404 |
| GET | `/api/veterinarios/{veterinarioId}/citas` | Ruta anidada: citas de un veterinario | 200, 404 |
| POST | `/api/citas` | Crea una cita | 201, 400, 409 |
| PUT | `/api/citas/{id}` | Actualiza una cita | 204, 400, 404, 409 |
| DELETE | `/api/citas/{id}` | Elimina una cita | 204, 404 |

22 endpoints en total (minimo pedido: 10).

## Base de datos (Database First)

La base se crea con un script SQL y el modelo se genera con `Scaffold-DbContext` a partir de esa base (no con migraciones Code First).

1. Ejecutar `Database/create_veterinaria.sql` contra la instancia SQL Server local (crea `VeterinariaDB`, tablas y datos de ejemplo).
2. Generar/regenerar el modelo con:

```
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=localhost\SQLEXPRESS;Database=VeterinariaDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c VeterinariaContext --context-dir Data --data-annotations
```

Cuando el esquema evoluciona (se agrego `Veterinarios`/`Citas` despues de la primera version), el flujo es: actualizar el script SQL, correrlo, y repetir el scaffold con `--force` para regenerar los 4 modelos desde cero. Tras cada scaffold se ajustan a mano: Data Annotations adicionales (`[Required]`, `[RegularExpression]`, `[EmailAddress]`, `[Phone]`), navegacion nula (`Tipo?`) en el lado "hijo" de cada relacion, `[JsonIgnore]` en las colecciones del lado "padre", y se remueve la cadena de conexion embebida en `VeterinariaContext` (se inyecta por configuracion en `Program.cs`).

## Comandos usados (orden real)

```
dotnet new webapi -n Lab12 --use-controllers          # (ya generado por Visual Studio)
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore.SwaggerUI
dotnet build
sqlcmd -S "localhost\SQLEXPRESS" -i "Database/create_veterinaria.sql"
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Server=localhost\SQLEXPRESS;Database=VeterinariaDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -c VeterinariaContext --context-dir Data --data-annotations
dotnet run
```

## Cadena de conexion

Local (desarrollo), en `appsettings.Development.json`:

```
Server=localhost\SQLEXPRESS;Database=VeterinariaDB;Trusted_Connection=True;TrustServerCertificate=True
```

En `appsettings.json` (lo que se publica) la cadena queda vacia; en produccion se configura por variable de entorno `ConnectionStrings__VeterinariaDb` segun el proveedor de hosting elegido. No se sube ninguna cadena real al repositorio.

## CORS y Swagger

- CORS habilitado solo para un origen concreto, configurable en `appsettings.json` -> `Cors:AllowedOrigin` (no se usa `AllowAnyOrigin`).
- Swagger (documento OpenAPI generado por `Microsoft.AspNetCore.OpenApi` + UI de `Swashbuckle.AspNetCore.SwaggerUI`) disponible en `/swagger` tanto en desarrollo como en produccion.

## Despliegue en la nube

Proveedor elegido: **MonsterASP.NET** (hosting ASP.NET Core + MSSQL en el mismo panel, plan gratuito, soporta .NET 8-11). Se justifica porque evita mezclar dos proveedores distintos para API y BD, y el flujo de publicacion es nativo de Visual Studio (Web Deploy), sin tocar Docker ni YAML de CI/CD.

### Pasos

1. **Crear el sitio** en el Panel de Control de MonsterASP.NET, eligiendo .NET 10 como version del framework.
2. **Crear la base MSSQL** desde el panel (Databases). El panel asigna su propio nombre de base y usuario; no hace falta que coincida con `VeterinariaDB`.
3. **Cargar el esquema**: correr `Database/create_veterinaria_cloud.sql` (sin `CREATE DATABASE`/`USE`, a diferencia del script local) contra esa base, via el editor T-SQL del panel, WebMSSQL o SSMS (host, usuario y password estan en Databases -> la base -> Users -> Remote access).
4. **Configurar la cadena de conexion**: en el sitio -> Scripting -> Environment variables, agregar `ConnectionStrings__VeterinariaDb` con la cadena de conexion **local** que da el panel para esa base (no la de acceso remoto). El doble guion bajo (`__`) es la convencion que ASP.NET Core usa para mapear a `ConnectionStrings:VeterinariaDb` en `appsettings.json`; no hace falta tocar el codigo.
5. Opcional: agregar tambien `Cors__AllowedOrigin` si el origen final difiere del placeholder `https://localhost:4200` en `appsettings.json`.
6. **Habilitar WebDeploy** (sitio -> Deploy -> tarjeta de acceso Visual Studio) y descargar el `.publishSettings`.
7. **Publicar desde Visual Studio**: boton derecho sobre el proyecto -> Publish -> Import profile -> seleccionar el `.publishSettings` descargado -> Publish. En la pantalla de publish, las secciones "Database"/"Entity Framework Migrations" se pueden dejar vacias (no se usan migraciones, el modelo es Database First).
8. Reiniciar el sitio tras guardar la variable de entorno (los cambios aplican en el siguiente arranque de la app).
9. Verificar `https://<tu-sitio>.monsterasp.net/swagger` y repetir las pruebas de `Lab12.http` contra esa URL (no localhost).

Enlace de la API publicada y capturas de las pruebas: pendientes hasta completar el deploy.

## Pruebas

Ver `Lab12.http` para las solicitudes de prueba de cada endpoint y cada codigo de respuesta (200, 201, 204, 400, 404, 409).
