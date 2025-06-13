using System.ComponentModel.DataAnnotations;

namespace PrestamosApi.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [Required]
        public int IdPrestamo { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MontoPagado { get; set; }

        [Required]
        public string MetodoPago { get; set; } = "EFECTIVO"; // o TRANSFERENCIA

        public string? Referencia { get; set; }
        public string? Comentario { get; set; }
    }
}
