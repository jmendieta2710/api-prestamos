// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using PrestamosApi.Models; // Asegúrate de incluir el namespace para tu modelo Usuario

namespace PrestamosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tus DbSet existentes
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PagoDetalle> PagoDetalles { get; set; }
        public DbSet<PrestamoCuota> PrestamoCuotas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } // ¡Añade esta línea para el modelo Usuario!

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tus configuraciones existentes de claves primarias
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
            modelBuilder.Entity<Prestamo>().HasKey(p => p.IdPrestamo);
            modelBuilder.Entity<Pago>().HasKey(p => p.IdPago);
            modelBuilder.Entity<PagoDetalle>().HasKey(p => p.IdDetalle);
            modelBuilder.Entity<PrestamoCuota>().HasKey(p => p.IdCuota);
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario); // ¡Añade esta línea para Usuario!

            // Si tienes la tabla PagoDetalle en singular en la DB y tu modelo es PagoDetalle
            modelBuilder.Entity<PagoDetalle>().ToTable("PagoDetalle");

            // Configuración para tipos decimales (para evitar los warnings que vimos antes)
            // Esto es opcional, pero buena práctica
            modelBuilder.Entity<Pago>().Property(p => p.MontoPagado).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PagoDetalle>().Property(p => p.MontoAplicado).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Prestamo>().Property(p => p.InteresAnual).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Prestamo>().Property(p => p.MontoOriginal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PrestamoCuota>().Property(p => p.CapitalCuota).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PrestamoCuota>().Property(p => p.InteresCuota).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PrestamoCuota>().Property(p => p.MontoCuota).HasColumnType("decimal(18,2)");
        }
    }
}