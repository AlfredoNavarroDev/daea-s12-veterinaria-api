# Notas — mascotas

## Dependencias

- Usa `VeterinariaContext` directamente, sin repositorio.
- `PostMascota` y `PutMascota` consultan `_context.Propietarios` (entidad de la otra unidad) para validar la FK — mismo patrón cruzado que `PropietariosController.DeletePropietario` usa sobre `Mascotas`.

## Notas

- No hay archivos de test automatizados para este controlador; la evidencia de pruebas vive en `Lab12.http`.
