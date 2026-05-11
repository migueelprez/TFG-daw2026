namespace TFG.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public ApplicationUser? Usuario { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public double Total { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Pagado, Cancelado
        public string? StripeSessionId { get; set; }
        public ICollection<LineaPedido> Lineas { get; set; } = new List<LineaPedido>();
    }
}
