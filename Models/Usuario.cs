// Models/Usuario.cs
using System.ComponentModel.DataAnnotations;

namespace PrestamosApi.Models // Asegúrate de que el namespace sea 'PrestamosApi.Models'
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder los 50 caracteres.")]
        public string NombreUsuario { get; set; } = string.Empty; // Inicialización para evitar warning CS8618

        [Required(ErrorMessage = "La contraseña hasheada es requerida.")]
        public string PasswordHash { get; set; } = string.Empty; // Inicialización para evitar warning CS8618

        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres.")]
        public string? Email { get; set; } // Hacemos Email anulable

        [StringLength(20, ErrorMessage = "El rol no puede exceder los 20 caracteres.")]
        public string? Rol { get; set; } = "Usuario"; // Hacemos Rol anulable y le damos un valor por defecto
    }
}