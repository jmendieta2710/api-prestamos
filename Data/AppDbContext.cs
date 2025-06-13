using Microsoft.EntityFrameworkCore;
using PrestamosApi.Models;

namespace PrestamosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PagoDetalle> PagoDetalles { get; set; }
        public DbSet<PrestamoCuota> PrestamoCuotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Prestamo>().HasKey(p => p.IdPrestamo);
            modelBuilder.Entity<Pago>().HasKey(p => p.IdPago);
            modelBuilder.Entity<PagoDetalle>().HasKey(p => p.IdDetalle);
            modelBuilder.Entity<PrestamoCuota>().HasKey(p => p.IdCuota);       
            
        }
    }
}
