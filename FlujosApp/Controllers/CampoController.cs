using FlujosApp.Data;
using FlujosApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlujosApp.Controllers
{
    /// <summary>
    /// Controlador para la gestión de campos asociados a pasos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CampoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CampoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los campos registrados junto con su paso asociado.
        /// </summary>
        /// <returns>Lista de campos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campo>>> GetCampos()
        {
            return await _context.Campos
                .Include(c => c.Paso)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene un campo específico por su ID.
        /// </summary>
        /// <param name="id">ID del campo a consultar.</param>
        /// <returns>Campo encontrado.</returns>
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

        /// <summary>
        /// Crea un nuevo campo asociado a un paso existente.
        /// </summary>
        /// <param name="campo">Objeto Campo a crear.</param>
        /// <returns>Campo creado.</returns>
        [HttpPost]
        public async Task<ActionResult<Campo>> PostCampo(Campo campo)
        {
            if (string.IsNullOrWhiteSpace(campo.Nombre))
                return BadRequest("El nombre del campo es obligatorio.");

            if (campo.PasoId <= 0 || !await _context.Pasos.AnyAsync(p => p.Id == campo.PasoId))
                return BadRequest("El paso asociado no existe.");

            _context.Campos.Add(campo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampo), new { id = campo.Id }, campo);
        }

        /// <summary>
        /// Actualiza un campo existente.
        /// </summary>
        /// <param name="id">ID del campo a actualizar.</param>
        /// <param name="campo">Objeto Campo con los nuevos datos.</param>
        /// <returns>Código de estado HTTP.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampo(int id, Campo campo)
        {
            if (id != campo.Id)
                return BadRequest();

            var existingCampo = await _context.Campos.FindAsync(id);
            if (existingCampo == null)
                return NotFound();

            existingCampo.Nombre = campo.Nombre;
            existingCampo.Valor = campo.Valor;
            existingCampo.YaProcesado = campo.YaProcesado;
            existingCampo.PasoId = campo.PasoId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar el campo: {ex.Message}");
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina un campo por su ID.
        /// </summary>
        /// <param name="id">ID del campo a eliminar.</param>
        /// <returns>Código de estado HTTP.</returns>
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
