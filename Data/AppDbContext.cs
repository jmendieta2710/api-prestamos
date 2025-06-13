using Microsoft.EntityFrameworkCore;
using PrestamosApi.Models;

namespace PrestamosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; } // 👈🏼 AÑADIDO
        
        public DbSet<Pago> Pagos { get; set; } // 👈🏼 AÑADIDO

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Prestamo>().HasKey(p => p.IdPrestamo);
            modelBuilder.Entity<Pago>().HasKey(p => p.IdPago);
        }
    }
}
