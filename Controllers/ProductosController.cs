using Microsoft.AspNetCore.Mvc;
using TFG.Models;
using System.Collections.Generic;
using System.Linq;

namespace TFG.Controllers
{
    public class ProductosController : Controller
    {
        // Método para ver TODOS los productos (el que llama el botón principal)
        public IActionResult Index()
        {
            var todos = GetProductos();
            ViewBag.CategoriaActual = "Todos los productos";
            return View("Categoria", todos); // Reutilizamos la vista de Categoria
        }

        // Método para filtrar por categoría
        public IActionResult Categoria(string id)
        {
            var filtrados = GetProductos()
                .Where(p => p.Categoria.Equals(id, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            ViewBag.CategoriaActual = id;
            return View(filtrados);
        }

        private List<ProductoModel> GetProductos()
        {
            return new List<ProductoModel> {
                new ProductoModel { Id = 1, Nombre = "Espinillera Premium", Categoria = "Espinilleras", ImagenUrl = "005DM107012 copia.jpg", Descripcion = "Protección avanzada." },
                new ProductoModel { Id = 2, Nombre = "Cama Geriátrica", Categoria = "Mobiliario", ImagenUrl = "cama.jpg", Descripcion = "Máximo confort." }
            };
        }
    }
}