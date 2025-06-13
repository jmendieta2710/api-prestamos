namespace PrestamosApi.Models
{
    public class PrestamoCuota
    {
        public int IdCuota { get; set; }
        public int IdPrestamo { get; set; }
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCuota { get; set; }
        public decimal InteresCuota { get; set; }
        public decimal CapitalCuota { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaPago { get; set; }
    }
}
