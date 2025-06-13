// DTOs/LoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace PrestamosApi.DTOs // Asegúrate de que el namespace sea 'PrestamosApi.DTOs'
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string NombreUsuario { get; set; } = string.Empty; // Inicialización para evitar warning CS8618

        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Contrasena { get; set; } = string.Empty; // Inicialización para evitar warning CS8618
    }
}