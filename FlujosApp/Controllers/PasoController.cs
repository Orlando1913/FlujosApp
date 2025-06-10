using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    /// <summary>
    /// Controlador para la gestión de pasos dentro de un flujo.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PasoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PasoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los pasos registrados con sus relaciones.
        /// </summary>
        /// <returns>Lista de pasos con flujo, campos y dependencias.</returns>
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

        /// <summary>
        /// Obtiene un paso específico por su ID.
        /// </summary>
        /// <param name="id">ID del paso a buscar.</param>
        /// <returns>El paso correspondiente al ID especificado.</returns>
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

        /// <summary>
        /// Crea un nuevo paso.
        /// </summary>
        /// <param name="paso">Objeto Paso a crear.</param>
        /// <returns>El paso creado.</returns>
        [HttpPost]
        public async Task<ActionResult<Paso>> CreatePaso(Paso paso)
        {
            if (string.IsNullOrWhiteSpace(paso.Nombre))
                return BadRequest("El nombre del paso es obligatorio.");

            if (paso.Orden <= 0)
                return BadRequest("El orden del paso debe ser mayor que cero.");

            _context.Pasos.Add(paso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaso), new { id = paso.Id }, paso);
        }

        /// <summary>
        /// Actualiza un paso existente.
        /// </summary>
        /// <param name="id">ID del paso a actualizar.</param>
        /// <param name="paso">Objeto Paso con los nuevos datos.</param>
        /// <returns>Código de estado HTTP indicando el resultado.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaso(int id, Paso paso)
        {
            if (id != paso.Id)
                return BadRequest();

            var pasoExistente = await _context.Pasos.FindAsync(id);
            if (pasoExistente == null)
                return NotFound();

            pasoExistente.Nombre = paso.Nombre;
            pasoExistente.Orden = paso.Orden;
            pasoExistente.FlujoId = paso.FlujoId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar paso: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un paso por su ID.
        /// </summary>
        /// <param name="id">ID del paso a eliminar.</param>
        /// <returns>Código de estado HTTP.</returns>
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
