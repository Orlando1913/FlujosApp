using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CampoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campo>>> GetCampos()
        {
            return await _context.Campos
                .Include(c => c.Paso)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Campo>> GetCampo(int id)
        {
            var campo = await _context.Campos
                .Include(c => c.Paso)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campo == null)
                return NotFound();

            return campo;
        }

        [HttpPost]
        public async Task<ActionResult<Campo>> PostCampo(Campo campo)
        {
            _context.Campos.Add(campo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampo), new { id = campo.Id }, campo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampo(int id, Campo campo)
        {
            if (id != campo.Id)
                return BadRequest();

            _context.Entry(campo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Campos.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampo(int id)
        {
            var campo = await _context.Campos.FindAsync(id);
            if (campo == null)
                return NotFound();

            _context.Campos.Remove(campo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
