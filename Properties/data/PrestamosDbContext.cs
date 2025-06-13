using Microsoft.EntityFrameworkCore;

namespace PrestamosApi.Data
{
    public class PrestamosDbContext : DbContext
    {
        public PrestamosDbContext(DbContextOptions<PrestamosDbContext> options)
            : base(options)
        {
        }

        // Aquí defines los DbSets, que representan las tablas
        // Ejemplo:
        // public DbSet<Prestamo> Prestamos { get; set; }
    }
}
