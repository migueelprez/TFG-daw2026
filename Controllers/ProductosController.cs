using Microsoft.AspNetCore.Mvc;
using TFG.Data;

namespace TFG.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ver todos los productos
        public IActionResult Productos()
        {
            var todos = _context.Productos.ToList();
            ViewBag.CategoriaActual = "Todos los productos";
            return View("Categoria", todos);
        }

        // Filtrar por categoría
        public IActionResult Categoria(string id)
        {
            var filtrados = _context.Productos
                .Where(p => p.Categoria.ToLower() == id.ToLower())
                .ToList();

            ViewBag.CategoriaActual = id;
            return View(filtrados);
        }

        // Página dedicada al producto estrella: Espinillera
        public IActionResult Espinillera()
        {
            var espinilleras = _context.Productos
                .Where(p => p.Categoria == "Espinilleras")
                .ToList();

            return View(espinilleras);
        }
    }
}
