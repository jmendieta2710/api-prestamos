using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;
using PrestamosApi.Models;

namespace PrestamosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoCuotasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PrestamoCuotasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/prestamocuotas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrestamoCuota>>> GetCuotas()
        {
            return await _context.PrestamoCuotas.ToListAsync();
        }

        // GET: api/prestamocuotas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrestamoCuota>> GetCuota(int id)
        {
            var cuota = await _context.PrestamoCuotas.FindAsync(id);
            if (cuota == null)
                return NotFound();

            return cuota;
        }

        // POST: api/prestamocuotas
        [HttpPost]
        public async Task<ActionResult<PrestamoCuota>> CreateCuota(PrestamoCuota cuota)
        {
            var prestamoExiste = await _context.Prestamos.AnyAsync(p => p.IdPrestamo == cuota.IdPrestamo);
            if (!prestamoExiste)
                return BadRequest("El préstamo no existe.");

            _context.PrestamoCuotas.Add(cuota);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCuota), new { id = cuota.IdCuota }, cuota);
        }

        // PUT: api/prestamocuotas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCuota(int id, PrestamoCuota cuota)
        {
            if (id != cuota.IdCuota)
                return BadRequest();

            _context.Entry(cuota).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/prestamocuotas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuota(int id)
        {
            var cuota = await _context.PrestamoCuotas.FindAsync(id);
            if (cuota == null)
                return NotFound();

            _context.PrestamoCuotas.Remove(cuota);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
