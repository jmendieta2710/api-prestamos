using System.ComponentModel.DataAnnotations;

namespace PrestamosApi.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        public required string Nombre { get; set; }
        public required string DocumentoIdentidad { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
    }
}
