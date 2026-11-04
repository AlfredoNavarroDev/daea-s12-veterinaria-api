# Notas — Cita

La regla de negocio "un veterinario no puede tener dos citas en el mismo horario" **no está respaldada por ninguna constraint de BD** (no hay `UNIQUE (VeterinarioId, FechaHora)`) — vive exclusivamente en `CitasController` (`PostCita`/`PutCita`). Si en algún momento se escribiera directamente a la tabla por fuera de la API (otro proceso, un script, una migración mal pensada), esta regla no se respetaría. Sería una mejora razonable agregar el índice único a nivel de BD como respaldo, igual que `UQ_Propietarios_Dni` y `UQ_Veterinarios_Licencia` respaldan sus reglas de duplicado.
