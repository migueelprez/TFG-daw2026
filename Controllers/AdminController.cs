using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFG.Data;
using TFG.Models;

namespace TFG.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ─── DASHBOARD ───────────────────────────────────────────────────────────

        public IActionResult Index()
        {
            ViewBag.TotalProductos = _context.Productos.Count();
            ViewBag.TotalUsuarios  = _context.Usuarios.Count();
            ViewBag.TotalPedidos   = _context.Pedidos.Count();
            ViewBag.Ingresos       = _context.Pedidos
                                        .Where(p => p.Estado == "Pagado")
                                        .Sum(p => (double?)p.Total) ?? 0;
            ViewBag.SolicitudesPendientes = _context.SolicitudesPresupuesto
                                        .Count(s => s.Estado == "Pendiente");

            ViewBag.UltimosPedidos = _context.Pedidos
                                        .Include(p => p.Usuario)
                                        .OrderByDescending(p => p.FechaPedido)
                                        .Take(8)
                                        .ToList();
            return View();
        }

        // ─── GESTIÓN DE PRODUCTOS ────────────────────────────────────────────────

        public IActionResult Productos()
        {
            var productos = _context.Productos
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nombre)
                .ToList();
            return View(productos);
        }

        public IActionResult NuevoProducto()
        {
            return View("EditarProducto", new ProductoModel());
        }

        public IActionResult EditarProducto(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarProducto(ProductoModel model)
        {
            if (!ModelState.IsValid) return View("EditarProducto", model);

            if (model.Id == 0)
                _context.Productos.Add(model);
            else
                _context.Productos.Update(model);

            await _context.SaveChangesAsync();
            TempData["Exito"] = "Producto guardado correctamente.";
            return RedirectToAction("Productos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Producto eliminado.";
            }
            return RedirectToAction("Productos");
        }

        // ─── GESTIÓN DE USUARIOS ─────────────────────────────────────────────────

        public IActionResult Usuarios()
        {
            var usuarios = _context.Usuarios
                .OrderBy(u => u.Rol)
                .ThenBy(u => u.Nombre)
                .ToList();
            return View(usuarios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                usuario.Activo = !usuario.Activo;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Usuarios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarRol(int id, string rol)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null && (rol == "Admin" || rol == "Usuario"))
            {
                usuario.Rol = rol;
                await _context.SaveChangesAsync();
                TempData["Exito"] = $"Rol de {usuario.Nombre} cambiado a {rol}.";
            }
            return RedirectToAction("Usuarios");
        }

        // ─── ESTADÍSTICAS ────────────────────────────────────────────────────────

        public IActionResult Estadisticas()
        {
            var porCategoria = _context.Productos
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key, Total = g.Count() })
                .ToList();

            ViewBag.PorCategoria   = porCategoria;
            ViewBag.TotalPedidos   = _context.Pedidos.Count();
            ViewBag.PedidosPagados = _context.Pedidos.Count(p => p.Estado == "Pagado");
            ViewBag.Ingresos       = _context.Pedidos
                                        .Where(p => p.Estado == "Pagado")
                                        .Sum(p => (double?)p.Total) ?? 0;
            ViewBag.ProductosSinStock = _context.Productos.Count(p => p.Stock == 0);
            return View();
        }

        // ─── PEDIDOS (con exportación PDF) ───────────────────────────────────────

        public IActionResult Pedidos()
        {
            var pedidos = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Lineas)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();
            return View(pedidos);
        }

        public IActionResult DetallePedido(int id)
        {
            var pedido = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Lineas)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null) return NotFound();
            return View(pedido);
        }

        // ─── SOLICITUDES DE PRESUPUESTO ──────────────────────────────────────────

        public IActionResult Presupuestos()
        {
            var solicitudes = _context.SolicitudesPresupuesto
                .OrderByDescending(s => s.FechaSolicitud)
                .ToList();
            return View(solicitudes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoPresupuesto(int id, string estado)
        {
            var solicitud = _context.SolicitudesPresupuesto.Find(id);
            if (solicitud != null)
            {
                solicitud.Estado = estado;
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Estado actualizado correctamente.";
            }
            return RedirectToAction("Presupuestos");
        }
    }
}
