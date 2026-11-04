# Validación — citas

| Campo | Reglas |
|---|---|
| `FechaHora` | `[Required]` (`DateTime` no-nulo). Sin restricción de rango: una fecha pasada es válida (no hay chequeo de "fecha futura"). |
| `Motivo` | `[Required]`, `[StringLength(200)]` |
| `MascotaId` | `[Required]` (int no-nulo) |
| `VeterinarioId` | `[Required]` (int no-nulo) |
| `Mascota` | nav **nullable** (`Mascota?`) — a propósito, mismo motivo que `Mascota.Propietario` (ver `docs/backend/mascotas/dudas-recurrentes.md`) |
| `Veterinario` | nav **nullable** (`Veterinario?`), mismo motivo |

Ninguna Data Annotation puede expresar "el veterinario no tiene otra cita a esa hora" ni "la mascota/veterinario referenciados existen" — ambas se resuelven en el controlador contra el propio `DbContext` (ver `flujo.md`).
