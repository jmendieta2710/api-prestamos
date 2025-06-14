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
public async Task<ActionResult<object>> Login(LoginRequest request) // CAMBIAMOS ActionResult<string> a ActionResult<object>
{
    var usuario = await _context.Usuarios
    .FirstOrDefaultAsync(u => EF.Functions.Like(u.NombreUsuario, request.NombreUsuario));

    if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Contrasena, usuario.PasswordHash))
    {
        return Unauthorized("Credenciales inválidas.");
    }

    // --- Generar el Token JWT ---
    var claims = new[] // Las "declaraciones" o "claims" del token (información sobre el usuario)
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()), // ID del usuario
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),                  // Nombre de usuario
        new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")                // Rol del usuario
        // Puedes añadir más claims aquí, como email, etc.
    };

    var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
    var jwtIssuer = _configuration["Jwt:Issuer"];
    var jwtAudience = _configuration["Jwt:Audience"];

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(1), // El token expira en 1 hora (ajusta esto según tus necesidades)
        SigningCredentials = creds,
        Issuer = jwtIssuer,
        Audience = jwtAudience
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    // Devolvemos el token como un objeto JSON
    return Ok(new { token = tokenHandler.WriteToken(token), message = "Login exitoso. ¡Bienvenido!" }); 
}
    }
}