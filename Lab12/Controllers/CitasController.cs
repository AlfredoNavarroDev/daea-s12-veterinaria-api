using Lab12.Data;
using Lab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Controllers;

[Route("api/citas")]
[ApiController]
public class CitasController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public CitasController(VeterinariaContext context)
    {
        _context = context;
    }

    // GET api/citas?mascotaId=1&veterinarioId=2
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitas(
        [FromQuery] int? mascotaId,
        [FromQuery] int? veterinarioId)
    {
        var query = _context.Citas
            .Include(c => c.Mascota)
            .Include(c => c.Veterinario)
            .AsNoTracking()
            .AsQueryable();

        if (mascotaId.HasValue)
        {
            query = query.Where(c => c.MascotaId == mascotaId.Value);
        }

        if (veterinarioId.HasValue)
        {
            query = query.Where(c => c.VeterinarioId == veterinarioId.Value);
        }

        var citas = await query.ToListAsync();

        return Ok(citas);
    }

    // GET api/citas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Cita>> GetCita(int id)
    {
        var cita = await _context.Citas
            .Include(c => c.Mascota)
            .Include(c => c.Veterinario)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cita == null)
        {
            return NotFound();
        }

        return Ok(cita);
    }

    // GET api/mascotas/5/citas  (ruta anidada)
    [HttpGet("/api/mascotas/{mascotaId}/citas")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasPorMascota(int mascotaId)
    {
        var mascotaExiste = await _context.Mascotas.AsNoTracking().AnyAsync(m => m.Id == mascotaId);
        if (!mascotaExiste)
        {
            return NotFound(new { mensaje = $"No existe la mascota con id {mascotaId}." });
        }

        var citas = await _context.Citas
            .AsNoTracking()
            .Where(c => c.MascotaId == mascotaId)
            .ToListAsync();

        return Ok(citas);
    }

    // GET api/veterinarios/5/citas  (ruta anidada)
    [HttpGet("/api/veterinarios/{veterinarioId}/citas")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasPorVeterinario(int veterinarioId)
    {
        var veterinarioExiste = await _context.Veterinarios.AsNoTracking().AnyAsync(v => v.Id == veterinarioId);
        if (!veterinarioExiste)
        {
            return NotFound(new { mensaje = $"No existe el veterinario con id {veterinarioId}." });
        }

        var citas = await _context.Citas
            .AsNoTracking()
            .Where(c => c.VeterinarioId == veterinarioId)
            .ToListAsync();

        return Ok(citas);
    }

    // POST api/citas
    [HttpPost]
    public async Task<ActionResult<Cita>> PostCita(Cita cita)
    {
        var mascotaExiste = await _context.Mascotas.AnyAsync(m => m.Id == cita.MascotaId);
        if (!mascotaExiste)
        {
            return BadRequest(new { mensaje = $"No existe la mascota con id {cita.MascotaId}." });
        }

        var veterinarioExiste = await _context.Veterinarios.AnyAsync(v => v.Id == cita.VeterinarioId);
        if (!veterinarioExiste)
        {
            return BadRequest(new { mensaje = $"No existe el veterinario con id {cita.VeterinarioId}." });
        }

        var horarioOcupado = await _context.Citas
            .AnyAsync(c => c.VeterinarioId == cita.VeterinarioId && c.FechaHora == cita.FechaHora);

        if (horarioOcupado)
        {
            return Conflict(new { mensaje = "El veterinario ya tiene una cita programada en esa fecha y hora." });
        }

        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCita), new { id = cita.Id }, cita);
    }

    // PUT api/citas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCita(int id, Cita cita)
    {
        if (id != cita.Id)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });
        }

        var existe = await _context.Citas.AsNoTracking().AnyAsync(c => c.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        var mascotaExiste = await _context.Mascotas.AnyAsync(m => m.Id == cita.MascotaId);
        if (!mascotaExiste)
        {
            return BadRequest(new { mensaje = $"No existe la mascota con id {cita.MascotaId}." });
        }

        var veterinarioExiste = await _context.Veterinarios.AnyAsync(v => v.Id == cita.VeterinarioId);
        if (!veterinarioExiste)
        {
            return BadRequest(new { mensaje = $"No existe el veterinario con id {cita.VeterinarioId}." });
        }

        var horarioOcupado = await _context.Citas
            .AnyAsync(c => c.VeterinarioId == cita.VeterinarioId && c.FechaHora == cita.FechaHora && c.Id != id);

        if (horarioOcupado)
        {
            return Conflict(new { mensaje = "El veterinario ya tiene otra cita programada en esa fecha y hora." });
        }

        _context.Entry(cita).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/citas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCita(int id)
    {
        var cita = await _context.Citas.FindAsync(id);
        if (cita == null)
        {
            return NotFound();
        }

        _context.Citas.Remove(cita);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
