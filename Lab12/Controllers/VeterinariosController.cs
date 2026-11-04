using Lab12.Data;
using Lab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Controllers;

[Route("api/veterinarios")]
[ApiController]
public class VeterinariosController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public VeterinariosController(VeterinariaContext context)
    {
        _context = context;
    }

    // GET api/veterinarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Veterinario>>> GetVeterinarios()
    {
        var veterinarios = await _context.Veterinarios
            .AsNoTracking()
            .ToListAsync();

        return Ok(veterinarios);
    }

    // GET api/veterinarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Veterinario>> GetVeterinario(int id)
    {
        var veterinario = await _context.Veterinarios
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veterinario == null)
        {
            return NotFound();
        }

        return Ok(veterinario);
    }

    // POST api/veterinarios
    [HttpPost]
    public async Task<ActionResult<Veterinario>> PostVeterinario(Veterinario veterinario)
    {
        var licenciaRepetida = await _context.Veterinarios
            .AnyAsync(v => v.Licencia == veterinario.Licencia);

        if (licenciaRepetida)
        {
            return Conflict(new { mensaje = $"Ya existe un veterinario con la licencia {veterinario.Licencia}." });
        }

        _context.Veterinarios.Add(veterinario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVeterinario), new { id = veterinario.Id }, veterinario);
    }

    // PUT api/veterinarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVeterinario(int id, Veterinario veterinario)
    {
        if (id != veterinario.Id)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });
        }

        var existe = await _context.Veterinarios.AsNoTracking().AnyAsync(v => v.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        var licenciaRepetida = await _context.Veterinarios
            .AnyAsync(v => v.Licencia == veterinario.Licencia && v.Id != id);

        if (licenciaRepetida)
        {
            return Conflict(new { mensaje = $"Ya existe otro veterinario con la licencia {veterinario.Licencia}." });
        }

        _context.Entry(veterinario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/veterinarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVeterinario(int id)
    {
        var veterinario = await _context.Veterinarios.FindAsync(id);
        if (veterinario == null)
        {
            return NotFound();
        }

        var tieneCitas = await _context.Citas.AnyAsync(c => c.VeterinarioId == id);
        if (tieneCitas)
        {
            return Conflict(new { mensaje = "No se puede eliminar: el veterinario tiene citas registradas." });
        }

        _context.Veterinarios.Remove(veterinario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
