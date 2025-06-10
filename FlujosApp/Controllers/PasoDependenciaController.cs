using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasoDependenciaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PasoDependenciaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PasoDependencia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PasoDependencia>>> GetDependencias()
        {
            return await _context.PasoDependencias
                .Include(d => d.Paso)
                .Include(d => d.DependeDePaso)
                .ToListAsync();
        }

        // GET: api/PasoDependencia/5/3
        [HttpGet("{pasoId:int}/{dependeDePasoId:int}")]
        public async Task<ActionResult<PasoDependencia>> GetDependencia(int pasoId, int dependeDePasoId)
        {
            var dependencia = await _context.PasoDependencias
                .Include(d => d.Paso)
                .Include(d => d.DependeDePaso)
                .FirstOrDefaultAsync(d => d.PasoId == pasoId && d.DependeDePasoId == dependeDePasoId);

            if (dependencia == null)
                return NotFound();

            return dependencia;
        }

        // POST: api/PasoDependencia
        [HttpPost]
        public async Task<ActionResult<PasoDependencia>> PostDependencia(PasoDependencia dependencia)
        {
            _context.PasoDependencias.Add(dependencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDependencia), new { pasoId = dependencia.PasoId, dependeDePasoId = dependencia.DependeDePasoId }, dependencia);
        }

        // PUT: api/PasoDependencia/5/3
        [HttpPut("{pasoId:int}/{dependeDePasoId:int}")]
        public async Task<IActionResult> PutDependencia(int pasoId, int dependeDePasoId, PasoDependencia dependencia)
        {
            if (pasoId != dependencia.PasoId || dependeDePasoId != dependencia.DependeDePasoId)
                return BadRequest();

            _context.Entry(dependencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PasoDependenciaExists(pasoId, dependeDePasoId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/PasoDependencia/5/3
        [HttpDelete("{pasoId:int}/{dependeDePasoId:int}")]
        public async Task<IActionResult> DeleteDependencia(int pasoId, int dependeDePasoId)
        {
            var dependencia = await _context.PasoDependencias.FindAsync(pasoId, dependeDePasoId);
            if (dependencia == null)
                return NotFound();

            _context.PasoDependencias.Remove(dependencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> PasoDependenciaExists(int pasoId, int dependeDePasoId)
        {
            return await _context.PasoDependencias.AnyAsync(e =>
                e.PasoId == pasoId && e.DependeDePasoId == dependeDePasoId);
        }
    }
}
