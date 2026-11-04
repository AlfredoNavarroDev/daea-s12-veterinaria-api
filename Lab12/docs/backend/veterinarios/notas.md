# Notas — veterinarios

## Dependencias

- Usa `VeterinariaContext` directamente, sin repositorio.
- `DeleteVeterinario` consulta `_context.Citas` (entidad de otra unidad) para el chequeo de dependientes.

## Notas

- No hay archivos de test automatizados para este controlador; la evidencia de pruebas vive en `Lab12.http`.
