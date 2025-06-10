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

        /// <summary>
        /// Obtiene todas las dependencias entre pasos.
        /// </summary>
        /// <returns>Lista de objetos PasoDependencia.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PasoDependencia>>> GetDependencias()
        {
            return await _context.PasoDependencias
                .Include(d => d.Paso)
                .Include(d => d.DependeDePaso)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una dependencia específica entre pasos.
        /// </summary>
        /// <param name="pasoId">ID del paso.</param>
        /// <param name="dependeDePasoId">ID del paso del cual depende.</param>
        /// <returns>Objeto PasoDependencia si existe.</returns>
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

        /// <summary>
        /// Verifica si un paso puede ser ejecutado con base en sus dependencias.
        /// </summary>
        /// <param name="pasoId">ID del paso a verificar.</param>
        /// <returns>True si puede ejecutarse, false en caso contrario.</returns>
        [HttpGet("puede-ejecutar/{pasoId}")]
        public async Task<ActionResult<bool>> PuedeEjecutarPaso(int pasoId)
        {
            var dependencias = await _context.PasoDependencias
                .Where(d => d.PasoId == pasoId)
                .Include(d => d.DependeDePaso)
                    .ThenInclude(p => p.Campos)
                .ToListAsync();

            if (dependencias.Count == 0)
                return true;

            bool puedeEjecutar = dependencias.All(d =>
                d.DependeDePaso.Campos.All(c => c.YaProcesado));

            return puedeEjecutar;
        }

        /// <summary>
        /// Crea una nueva relación de dependencia entre pasos.
        /// </summary>
        /// <param name="dependencia">Objeto PasoDependencia.</param>
        /// <returns>Dependencia creada.</returns>
        [HttpPost]
        public async Task<ActionResult<PasoDependencia>> PostDependencia(PasoDependencia dependencia)
        {
            var pasoExiste = await _context.Pasos.AnyAsync(p => p.Id == dependencia.PasoId);
            var dependeDePasoExiste = await _context.Pasos.AnyAsync(p => p.Id == dependencia.DependeDePasoId);

            if (!pasoExiste || !dependeDePasoExiste)
                return BadRequest("Uno o ambos pasos no existen.");

            if (dependencia.PasoId == dependencia.DependeDePasoId)
                return BadRequest("Un paso no puede depender de sí mismo.");

            var yaExiste = await _context.PasoDependencias.AnyAsync(d =>
                d.PasoId == dependencia.PasoId && d.DependeDePasoId == dependencia.DependeDePasoId);

            if (yaExiste)
                return Conflict("La dependencia ya existe.");

            _context.PasoDependencias.Add(dependencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDependencia),
                new { pasoId = dependencia.PasoId, dependeDePasoId = dependencia.DependeDePasoId }, dependencia);
        }

        /// <summary>
        /// Actualiza una relación de dependencia existente.
        /// </summary>
        /// <param name="pasoId">ID del paso.</param>
        /// <param name="dependeDePasoId">ID del paso del cual depende.</param>
        /// <param name="nuevaDependencia">Datos actualizados de la dependencia.</param>
        /// <returns>Respuesta vacía si la actualización fue exitosa.</returns>
        [HttpPut("{pasoId:int}/{dependeDePasoId:int}")]
        public async Task<IActionResult> PutDependencia(int pasoId, int dependeDePasoId, PasoDependencia nuevaDependencia)
        {
            if (pasoId != nuevaDependencia.PasoId || dependeDePasoId != nuevaDependencia.DependeDePasoId)
                return BadRequest();

            var dependenciaExistente = await _context.PasoDependencias
                .FirstOrDefaultAsync(d => d.PasoId == pasoId && d.DependeDePasoId == dependeDePasoId);

            if (dependenciaExistente == null)
                return NotFound();


            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Elimina una relación de dependencia entre pasos.
        /// </summary>
        /// <param name="pasoId">ID del paso.</param>
        /// <param name="dependeDePasoId">ID del paso del cual depende.</param>
        /// <returns>Respuesta vacía si la eliminación fue exitosa.</returns>
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

        /// <summary>
        /// Verifica si una relación de dependencia existe.
        /// </summary>
        /// <param name="pasoId">ID del paso.</param>
        /// <param name="dependeDePasoId">ID del paso del cual depende.</param>
        /// <returns>True si la relación existe, false en caso contrario.</returns>
        private async Task<bool> PasoDependenciaExists(int pasoId, int dependeDePasoId)
        {
            return await _context.PasoDependencias.AnyAsync(e =>
                e.PasoId == pasoId && e.DependeDePasoId == dependeDePasoId);
        }
    }
}
