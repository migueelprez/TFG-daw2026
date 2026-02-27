namespace TFG.Models
{
    public class ProductoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }
        public string Categoria { get; set; } // Ejemplo: "Espinilleras", "Mobiliario"
        public double Precio { get; set; }
    }
}