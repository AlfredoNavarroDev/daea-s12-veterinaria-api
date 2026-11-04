# Validación — veterinarios

| Campo | Reglas |
|---|---|
| `Nombres` | `[Required]`, `[StringLength(80)]` |
| `Apellidos` | `[Required]`, `[StringLength(80)]` |
| `Licencia` | `[Required]`, `[StringLength(10)]`, `[RegularExpression(@"^[A-Z]{4}\d{4}$")]` (formato `CMVP0001`: 4 letras mayúsculas + 4 dígitos), `[Unicode(false)]`. Además `UNIQUE` en BD (`UQ_Veterinarios_Licencia`), pero el `409` por duplicado lo da el controlador con `AnyAsync`, no la constraint (mismo patrón que `Dni` en `Propietario`, ver `docs/backend/propietarios/validacion.md`). |
| `Especialidad` | `[StringLength(60)]`, nullable |
| `Telefono` | `[Phone]`, `[StringLength(9)]`, nullable |

`Licencia` está mapeada a `VARCHAR(10)` en la BD, que por sí sola aceptaría cualquier combinación de hasta 10 caracteres ASCII — el formato exacto "4 letras + 4 dígitos" lo impone únicamente el `RegularExpression` del modelo.
