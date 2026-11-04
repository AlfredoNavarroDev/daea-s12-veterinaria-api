# Notas — Mascota

El modelo C# (`Mascota.Propietario` como `Propietario?`) y la BD (`PropietarioId NOT NULL`, FK obligatoria) no están realmente en desacuerdo: la columna sigue siendo obligatoria, solo la propiedad de navegación en memoria puede quedar sin cargar (nula) hasta que un `Include` la popule. Es una nulabilidad de conveniencia para el serializador JSON y el binding del body, no una relación opcional real. Ver `docs/backend/mascotas/dudas-recurrentes.md`.
