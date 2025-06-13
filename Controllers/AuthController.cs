// Controllers/AuthController.cs
using Microsoft.IdentityModel.Tokens; // Para SecurityTokenDescriptor, SymmetricSecurityKey
using System.IdentityModel.Tokens.Jwt; // Para JwtSecurityTokenHandler
using System.Security.Claims;         // Para Claims
using System.Text;                    // Para Encoding.UTF8
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;      // Para AppDbContext
using PrestamosApi.Models;    // Para el modelo Usuario
using PrestamosApi.DTOs;     // Para el DTO LoginRequest
using BCrypt.Net;            // Para el hashing de contraseñas (asegúrate de haberlo instalado)


namespace PrestamosApi.Controllers // Asegúrate de que el namespace sea 'PrestamosApi.Controllers'
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/Auth
    public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration; // Añade esto

    public AuthController(AppDbContext context, IConfiguration configuration) // Modifica el constructor
    {
        _context = context;
        _configuration = configuration; // Asigna la configuración
    }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="request">Credenciales del nuevo usuario (nombre de usuario y contraseña).</param>
        /// <returns>El usuario creado si el registro es exitoso.</returns>
        [HttpPost("register")] // Endpoint: POST /api/Auth/register
        public async Task<ActionResult<Usuario>> Register(LoginRequest request)
        {
            // 1. Verificar si el nombre de usuario ya existe
            if (await _context.Usuarios.AnyAsync(u => u.NombreUsuario == request.NombreUsuario))
            {
                return BadRequest("El nombre de usuario ya está en uso. Por favor, elija otro.");
            }

            // 2. Hashear la contraseña antes de guardarla.
            // BCrypt.Net.BCrypt.HashPassword genera un hash seguro de la contraseña.
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Contrasena);

            // 3. Crear una nueva instancia del modelo Usuario con los datos de la solicitud
            var nuevoUsuario = new Usuario
            {
                NombreUsuario = request.NombreUsuario,
                PasswordHash = passwordHash,
                Email = $"{request.NombreUsuario}@example.com", // Ejemplo de email generado
                Rol = "Usuario" // Rol por defecto, puedes ajustarlo según tus necesidades
            };

            // 4. Añadir el nuevo usuario al contexto y guardar los cambios en la base de datos
            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            // 5. Devolver una respuesta 201 Created con el usuario creado
            return StatusCode(201, nuevoUsuario); // HTTP 201 Created
        }

        /// <summary>
        /// Autentica un usuario y, si las credenciales son válidas, devuelve un mensaje de éxito.
        /// (En el siguiente paso, devolverá un token JWT para la autenticación).
        /// </summary>
        /// <param name="request">Credenciales del usuario (nombre de usuario y contraseña).</param>
        /// <returns>Mensaje de éxito o error de autenticación.</returns>
        [HttpPost("login")] // Endpoint: POST /api/Auth/login
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            // 1. Buscar el usuario en la base de datos por el nombre de usuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == request.NombreUsuario);

            // 2. Si el usuario no se encuentra, devolver un error de no autorizado.
            // Es buena práctica no dar pistas específicas sobre si falló el usuario o la contraseña.
            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas."); // HTTP 401 Unauthorized
            }

            // 3. Verificar la contraseña ingresada contra el hash almacenado en la base de datos.
            // BCrypt.Net.BCrypt.Verify compara la contraseña en texto plano con el hash.
            bool passwordEsCorrecta = BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.PasswordHash);

            // 4. Si la contraseña no coincide, devolver un error de no autorizado
            if (!passwordEsCorrecta)
            {
                return Unauthorized("Credenciales inválidas."); // HTTP 401 Unauthorized
            }

            // 5. Si las credenciales son válidas, el login es exitoso.
            // *** IMPORTANTE: Aquí es donde, en el SIGUIENTE PASO, GENERAREMOS Y DEVOLVEREMOS UN TOKEN JWT. ***
            return Ok("Login exitoso. ¡Bienvenido!"); // HTTP 200 OK
        }
    }
}