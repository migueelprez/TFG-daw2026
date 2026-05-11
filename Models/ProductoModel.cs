namespace TFG.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public double Precio { get; set; }
        public int Stock { get; set; } = 50;
    }
}
