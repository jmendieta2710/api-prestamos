using System.ComponentModel.DataAnnotations;

namespace PrestamosApi.Models
{
    public class PagoDetalle
    {
        [Key]
        public int IdDetalle { get; set; }

        [Required]
        public int IdPago { get; set; }

        [Required]
        public int IdCuota { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MontoAplicado { get; set; }
    }
}
