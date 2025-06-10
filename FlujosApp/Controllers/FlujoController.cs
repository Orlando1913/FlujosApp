using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlujoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlujoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/flujo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flujo>>> GetFlujos()
        {
            return await _context.Flujos.ToListAsync();
        }

        // GET: api/flujo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flujo>> GetFlujo(int id)
        {
            var flujo = await _context.Flujos.FindAsync(id);

            if (flujo == null)
            {
                return NotFound();
            }

            return flujo;
        }

        // POST: api/flujo
        [HttpPost]
        public async Task<ActionResult<Flujo>> CreateFlujo(Flujo flujo)
        {
            _context.Flujos.Add(flujo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlujo), new { id = flujo.Id }, flujo);
        }

        // PUT: api/flujo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlujo(int id, Flujo flujo)
        {
            if (id != flujo.Id)
            {
                return BadRequest();
            }

            _context.Entry(flujo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Flujos.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/flujo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlujo(int id)
        {
            var flujo = await _context.Flujos.FindAsync(id);
            if (flujo == null)
            {
                return NotFound();
            }

            _context.Flujos.Remove(flujo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
