using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PasoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/paso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paso>>> GetPasos()
        {
            return await _context.Pasos
                .Include(p => p.Flujo)
                .Include(p => p.Campos)
                .Include(p => p.Dependencias)
                .Include(p => p.DependeDeEste)
                .ToListAsync();
        }

        // GET: api/paso/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Paso>> GetPaso(int id)
        {
            var paso = await _context.Pasos
                .Include(p => p.Flujo)
                .Include(p => p.Campos)
                .Include(p => p.Dependencias)
                .Include(p => p.DependeDeEste)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paso == null)
                return NotFound();

            return paso;
        }

        // POST: api/paso
        [HttpPost]
        public async Task<ActionResult<Paso>> CreatePaso(Paso paso)
        {
            _context.Pasos.Add(paso);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPaso), new { id = paso.Id }, paso);
        }

        // PUT: api/paso/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaso(int id, Paso paso)
        {
            if (id != paso.Id)
                return BadRequest();

            _context.Entry(paso).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/paso/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaso(int id)
        {
            var paso = await _context.Pasos.FindAsync(id);
            if (paso == null)
                return NotFound();

            _context.Pasos.Remove(paso);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
