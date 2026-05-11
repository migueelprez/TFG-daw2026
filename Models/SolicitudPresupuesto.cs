using System.ComponentModel.DataAnnotations;

namespace TFG.Models
{
    public class SolicitudPresupuesto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string NombreContacto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la residencia es obligatorio.")]
        public string Residencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "Describe los productos que necesitas.")]
        public string Detalle { get; set; } = string.Empty;

        public int NumResidentes { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Pendiente"; // Pendiente / Enviado / Cerrado
    }
}
