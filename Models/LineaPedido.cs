namespace TFG.Models
{
    public class LineaPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
    }
}
