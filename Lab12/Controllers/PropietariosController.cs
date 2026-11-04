using Lab12.Data;
using Lab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab12.Controllers;

[Route("api/propietarios")]
[ApiController]
public class PropietariosController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public PropietariosController(VeterinariaContext context)
    {
        _context = context;
    }

    // GET api/propietarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietarios()
    {
        var propietarios = await _context.Propietarios
            .AsNoTracking()
            .ToListAsync();

        return Ok(propietarios);
    }

    // GET api/propietarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Propietario>> GetPropietario(int id)
    {
        var propietario = await _context.Propietarios
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (propietario == null)
        {
            return NotFound();
        }

        return Ok(propietario);
    }

    // POST api/propietarios
    [HttpPost]
    public async Task<ActionResult<Propietario>> PostPropietario(Propietario propietario)
    {
        var dniRepetido = await _context.Propietarios
            .AnyAsync(p => p.Dni == propietario.Dni);

        if (dniRepetido)
        {
            return Conflict(new { mensaje = $"Ya existe un propietario con el DNI {propietario.Dni}." });
        }

        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPropietario), new { id = propietario.Id }, propietario);
    }

    // PUT api/propietarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPropietario(int id, Propietario propietario)
    {
        if (id != propietario.Id)
        {
            return BadRequest(new { mensaje = "El id de la ruta no coincide con el id del cuerpo." });
        }

        var existe = await _context.Propietarios.AsNoTracking().AnyAsync(p => p.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        var dniRepetido = await _context.Propietarios
            .AnyAsync(p => p.Dni == propietario.Dni && p.Id != id);

        if (dniRepetido)
        {
            return Conflict(new { mensaje = $"Ya existe otro propietario con el DNI {propietario.Dni}." });
        }

        _context.Entry(propietario).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE api/propietarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePropietario(int id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        if (propietario == null)
        {
            return NotFound();
        }

        var tieneMascotas = await _context.Mascotas.AnyAsync(m => m.PropietarioId == id);
        if (tieneMascotas)
        {
            return Conflict(new { mensaje = "No se puede eliminar: el propietario tiene mascotas registradas." });
        }

        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
