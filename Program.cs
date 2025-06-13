using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;
using Microsoft.Data.SqlClient; // Necesario para el endpoint de prueba de DB

var builder = WebApplication.CreateBuilder(args);

// ====================================================================================
// CONFIGURACIÓN DE SERVICIOS (TODO ANTES DE builder.Build())
// ====================================================================================

// 🔧 1. Configurar cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🔧 2. Registrar el DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔧 3. Servicios para controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Configuración de JWT ---
// Obtiene la clave secreta y la información del emisor/audiencia desde appsettings.json
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// 🔧 4. Añadir servicios de Autenticación con JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,          // Validar el emisor (Issuer) del token
            ValidateAudience = true,        // Validar la audiencia (Audience) del token
            ValidateLifetime = true,        // Validar la fecha de expiración del token
            ValidateIssuerSigningKey = true, // Validar la clave de firma del emisor

            ValidIssuer = jwtIssuer,         // El emisor válido debe coincidir con el configurado
            ValidAudience = jwtAudience,     // La audiencia válida debe coincidir con la configurada
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // La clave de firma
        };
    });

// 🔧 5. Añadir el servicio de Autorización
builder.Services.AddAuthorization();

// ====================================================================================
// CONSTRUIR LA APLICACIÓN
// ====================================================================================
var app = builder.Build(); // ¡Esta es la ÚNICA llamada a builder.Build()!

// ====================================================================================
// CONFIGURACIÓN DE MIDDLEWARE (TODO DESPUÉS DE builder.Build())
// ====================================================================================

// 🔧 6. Middleware (Swagger en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔧 7. Middleware de Autenticación y Autorización
// ¡CRÍTICO: El orden es importante! UseAuthentication SIEMPRE antes de UseAuthorization.
app.UseAuthentication(); // Habilita el middleware de autenticación de JWT
app.UseAuthorization();  // Habilita el middleware de autorización

// 🔧 8. Mapear controladores
app.MapControllers();

// 🔧 9. Endpoint para probar conexión a la base de datos (para depuración)
app.MapGet("/testdb", () =>
{
    try
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        return Results.Ok("Conexión exitosa a la base de datos.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al conectar a la base de datos: {ex.Message}");
    }
});

// 🔧 10. Endpoint de ejemplo para clima (del template predeterminado)
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

// ✅ Record para el clima
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}