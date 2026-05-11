namespace TFG.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Usuario"; // "Admin" o "Usuario"
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
