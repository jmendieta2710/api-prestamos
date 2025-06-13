using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;
using PrestamosApi.Models;

namespace PrestamosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class   PagoDetalleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagoDetalleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDetalle>>> GetPagoDetalle()
        {
            return await _context.PagoDetalles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDetalle>> GetPagoDetalle(int id)
        {
            var pagodetalle = await _context.PagoDetalles.FindAsync(id);
            if (pagodetalle == null)
                return NotFound();
            return pagodetalle;
        }

        [HttpPost]
        public async Task<ActionResult<PagoDetalle>> CreatePago(PagoDetalle pagodetalle)
        {
            _context.PagoDetalles.Add(pagodetalle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPagoDetalle), new { id = pagodetalle.IdDetalle }, pagodetalle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePagoDetalle(int id, PagoDetalle pagodetalle)
        {
            if (id != pagodetalle.IdDetalle)
                return BadRequest();

            _context.Entry(pagodetalle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagoDetalle(int id)
        {
            var pagodetalle = await _context.PagoDetalles.FindAsync(id);
            if (pagodetalle == null)
                return NotFound();

            _context.PagoDetalles.Remove(pagodetalle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
