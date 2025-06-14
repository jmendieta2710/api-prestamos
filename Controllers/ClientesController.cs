using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;
using PrestamosApi.Models;
using Microsoft.AspNetCore.Authorization; // ¡IMPORTANTE: Añadir este using!

namespace PrestamosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Opcional: Puedes poner [Authorize] a nivel de controlador si quieres que TODOS los métodos requieran autorización
    // [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        // Protegemos este endpoint para que solo usuarios autenticados puedan ver la lista de clientes
        [HttpGet]
        [Authorize] // <--- Añadido para requerir autenticación
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            // Puedes añadir una comprobación aquí si _context.Clientes es nulo, aunque EF Core generalmente maneja esto
            if (_context.Clientes == null)
            {
                return NotFound("La colección de clientes no está disponible.");
            }
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/{id}
        // Puedes proteger este también si quieres que la consulta por ID requiera autenticación
        [HttpGet("{id}")]
        [Authorize] // <--- Añadido para requerir autenticación
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound("La colección de clientes no está disponible.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");
            return cliente;
        }

        // POST: api/Clientes
        // Protegemos este endpoint para que solo usuarios autenticados puedan crear clientes
        [HttpPost]
        [Authorize] // <--- Añadido para requerir autenticación
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
        }

        // PUT: api/Clientes/{id}
        // Protegemos este endpoint para que solo usuarios autenticados puedan actualizar clientes
        [HttpPut("{id}")]
        [Authorize] // <--- Añadido para requerir autenticación
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return BadRequest("El ID de la ruta no coincide con el ID del cliente.");

            _context.Entry(cliente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound("Cliente no encontrado para actualizar.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/Clientes/{id}
        // Protegemos este endpoint para que solo usuarios autenticados puedan eliminar clientes
        [HttpDelete("{id}")]
        [Authorize] // <--- Añadido para requerir autenticación
        public async Task<IActionResult> DeleteCliente(int id)
        {
            if (_context.Clientes == null)
            {
                return NotFound("La colección de clientes no está disponible.");
            }
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return (_context.Clientes?.Any(e => e.IdCliente == id)).GetValueOrDefault();
        }
    }
}