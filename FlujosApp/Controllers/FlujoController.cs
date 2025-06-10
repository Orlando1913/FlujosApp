using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    /// <summary>
    /// Controlador para gestionar flujos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FlujoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlujoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los flujos registrados.
        /// </summary>
        /// <returns>Una lista de flujos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flujo>>> GetFlujos()
        {
            return await _context.Flujos.ToListAsync();
        }

        /// <summary>
        /// Obtiene un flujo específico por su ID.
        /// </summary>
        /// <param name="id">ID del flujo a buscar.</param>
        /// <returns>El flujo correspondiente al ID.</returns>
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

        /// <summary>
        /// Crea un nuevo flujo.
        /// </summary>
        /// <param name="flujo">Objeto Flujo a crear.</param>
        /// <returns>El flujo creado.</returns>
        [HttpPost]
        public async Task<ActionResult<Flujo>> CreateFlujo(Flujo flujo)
        {
            if (string.IsNullOrWhiteSpace(flujo.Nombre))
                return BadRequest("El nombre del flujo es obligatorio.");

            _context.Flujos.Add(flujo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlujo), new { id = flujo.Id }, flujo);
        }

        /// <summary>
        /// Actualiza un flujo existente.
        /// </summary>
        /// <param name="id">ID del flujo a actualizar.</param>
        /// <param name="flujo">Objeto Flujo con los nuevos datos.</param>
        /// <returns>Código de estado HTTP.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlujo(int id, [FromBody] Flujo flujo)
        {
            if (id != flujo.Id)
                return BadRequest("El ID del flujo no coincide.");

            var flujoExistente = await _context.Flujos.FindAsync(id);

            if (flujoExistente == null)
                return NotFound();

            flujoExistente.Nombre = flujo.Nombre;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina un flujo por su ID.
        /// </summary>
        /// <param name="id">ID del flujo a eliminar.</param>
        /// <returns>Código de estado HTTP.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlujo(int id)
        {
            var flujo = await _context.Flujos
                .Include(f => f.Pasos)
                    .ThenInclude(p => p.Campos)
                .Include(f => f.Pasos)
                    .ThenInclude(p => p.Dependencias)
                .Include(f => f.Pasos)
                    .ThenInclude(p => p.DependeDeEste)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (flujo == null)
                return NotFound();

            _context.Flujos.Remove(flujo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
