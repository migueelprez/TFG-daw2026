namespace TFG.Models
{
    public class CarritoItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public double Subtotal => Precio * Cantidad;
    }
}
