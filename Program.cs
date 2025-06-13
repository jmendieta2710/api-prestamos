using Microsoft.EntityFrameworkCore;
using PrestamosApi.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// 🔧 1. Configurar cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 🔧 2. Registrar el DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🔧 3. Servicios para controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔧 4. Middleware (Swagger en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 🔧 5. Mapear controladores
app.MapControllers();

// 🔧 6. Endpoint para probar conexión a la base de datos
app.MapGet("/testdb", () =>
{
    using var connection = new SqlConnection(connectionString);
    connection.Open();
    return "Conexión exitosa a la base de datos";
});

// 🔧 7. Endpoint de ejemplo para clima
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
