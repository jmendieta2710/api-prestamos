using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;
using PrestamosApi.Models;

namespace PrestamosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();
            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return BadRequest();

            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
