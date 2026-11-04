# Validación — propietarios

| Campo | Reglas |
|---|---|
| `Nombres` | `[Required]`, `[StringLength(80)]` |
| `Apellidos` | `[Required]`, `[StringLength(80)]` |
| `Dni` | `[Required]`, `[StringLength(8, MinimumLength = 8)]`, `[RegularExpression(@"^\d{8}$")]`, `[Unicode(false)]`. Ademas, `UNIQUE` a nivel de BD (`UQ_Propietarios_Dni`) — pero el `409` por duplicado lo devuelve el controlador con una consulta `AnyAsync` explicita, **no** porque la BD rechace el `INSERT`/`UPDATE` (si el controlador no chequeara esto, el `SaveChangesAsync()` lanzaria una excepcion de constraint sin manejar, no un `409` limpio). |
| `Telefono` | `[Phone]`, `[StringLength(9)]`, nullable |
| `Email` | `[EmailAddress]`, `[StringLength(120)]`, nullable |
| `Direccion` | `[StringLength(150)]`, nullable |

`Dni` esta mapeado a `CHAR(8)` en la BD (ver script), que por si solo aceptaria letras; el formato "solo digitos" lo impone unicamente el `RegularExpression` del modelo, no la columna.
