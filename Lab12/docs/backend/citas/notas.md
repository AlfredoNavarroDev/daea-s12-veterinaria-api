# Notas — citas

## Dependencias

- Usa `VeterinariaContext` directamente, sin repositorio.
- `PostCita`/`PutCita` consultan `_context.Mascotas` y `_context.Veterinarios` (entidades de otras unidades) para validar ambas FK.
- `MascotasController.DeleteMascota` y `VeterinariosController.DeleteVeterinario` consultan `_context.Citas` en sentido inverso, para su propio chequeo de dependientes.

## Notas

- No hay archivos de test automatizados para este controlador; la evidencia de pruebas vive en `Lab12.http`.
- La regla de "horario ocupado" (ver `dudas-recurrentes.md`) es una igualdad exacta, no una verificación de superposición de rangos — queda como posible mejora si el enunciado llegara a pedir duración de cita.
