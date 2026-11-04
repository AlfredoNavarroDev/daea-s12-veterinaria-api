using Lab12.Data;
using Lab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Controllers;

[Route("api/mascotas")]
[ApiController]
public class MascotasController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public MascotasController(VeterinariaContext context)
    {
        _context = context;
    }

    // GET api/mascotas?especie=Perro&propietarioId=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotas(
        [FromQuery] string? especie,
        [FromQuery] int? propietarioId)
    {
        var query = _context.Mascotas
            .Include(m => m.Propietario)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(especie))
        {
            query = query.Where(m => m.Especie == especie);
        }

        if (propietarioId.HasValue)
        {
            query = query.Where(m => m.PropietarioId == propietarioId.Value);
        }

        var mascotas = await query.ToListAsync();

        return Ok(mascotas);
    }

    // GET api/mascotas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Mascota>> GetMascota(int id)
    {
        var mascota = await _context.Mascotas
            .Include(m => m.Propietario)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mascota == null)
        {
            return NotFound();
        }

        return Ok(mascota);
    }

    // GET api/propietarios/5/mascotas  (ruta anidada)
    [HttpGet("/api/propietarios/{propietarioId}/mascotas")]
    public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotasPorPropietario(int propietarioId)
    {
        var propietarioExiste = await _context.Propietarios.AsNoTracking().AnyAsync(p => p.Id == propietarioId);
        if (!propietarioExiste)
        {
            return NotFound(new { mensaje = $"No existe el propietario con id {propietarioId}." });
        }

        var mascotas = await _context.Mascotas
            .AsNoTracking()
            .Where(m => m.PropietarioId == propietarioId)
            .ToListAsync();

        return Ok(mascotas);
    }

    // POST api/mascotas
    [HttpPost]
    public async Task<ActionResult<Mascota>> PostMascota(Mascota mascota)
    {
        var propietarioExiste = await _context.Propietarios.AnyAsync(p => p.Id == mascota.PropietarioId);
        if (!propietarioExiste)
        {
            return BadRequest(new { mensaje = $"No existe el propietario con id {mascota.PropietarioId}." });
        }

        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMascota), new { id = mascota.Id }, mascota);
    }

    // PUT api/mascotas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMascota(int id, Mascota mascota)
    {
        if (id != mascota.Id)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });
        }

        var existe = await _context.Mascotas.AsNoTracking().AnyAsync(m => m.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        var propietarioExiste = await _context.Propietarios.AnyAsync(p => p.Id == mascota.PropietarioId);
        if (!propietarioExiste)
        {
            return BadRequest(new { mensaje = $"No existe el propietario con id {mascota.PropietarioId}." });
        }

        _context.Entry(mascota).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/mascotas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMascota(int id)
    {
        var mascota = await _context.Mascotas.FindAsync(id);
        if (mascota == null)
        {
            return NotFound();
        }

        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
