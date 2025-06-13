using System.ComponentModel.DataAnnotations;

namespace PrestamosApi.Models
{
    public class Prestamo
    {
        [Key]
        public int IdPrestamo { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [Required]
        public DateTime FechaPrestamo { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MontoOriginal { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal InteresAnual { get; set; }

        [Required]
        [Range(1, 360)]
        public int PlazoMeses { get; set; }

        [Required]
        public string FrecuenciaPago { get; set; } = "MENSUAL"; // o QUINCENAL, SEMANAL

        public string Estado { get; set; } = "ACTIVO";

        public string? Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
