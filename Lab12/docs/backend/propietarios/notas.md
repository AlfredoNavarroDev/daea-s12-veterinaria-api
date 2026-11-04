# Notas — propietarios

## Dependencias

- Usa `VeterinariaContext` (`Data/VeterinariaContext.cs`) directamente, sin repositorio intermedio.
- `DeletePropietario` consulta `_context.Mascotas` (tabla de la otra unidad) para el chequeo de dependientes — es el único punto donde este controlador toca una entidad que no es la suya.

## Notas

- No hay archivos de test automatizados para este controlador; la evidencia de pruebas vive en `Lab12.http` (manual, vía HTTP Client/REST Client).
