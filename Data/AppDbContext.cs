using Microsoft.EntityFrameworkCore;
using PrestamosApi.Models;

namespace PrestamosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
        }
    }
}
