using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using System.Text.Json;
using TFG.Data;
using TFG.Models;

namespace TFG.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CarritoController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ─── Helpers de sesión ───────────────────────────────────────────────────

        private List<CarritoItem> GetCarrito()
        {
            var json = HttpContext.Session.GetString("carrito");
            return json != null
                ? JsonSerializer.Deserialize<List<CarritoItem>>(json) ?? new()
                : new();
        }

        private void GuardarCarrito(List<CarritoItem> carrito)
        {
            HttpContext.Session.SetString("carrito", JsonSerializer.Serialize(carrito));
        }

        private int GetCarritoCount()
        {
            return GetCarrito().Sum(i => i.Cantidad);
        }

        // ─── GET /Carrito ─────────────────────────────────────────────────────────

        public IActionResult Index()
        {
            var carrito = GetCarrito();
            ViewBag.PublishableKey = _configuration["Stripe:PublishableKey"];
            return View(carrito);
        }

        // ─── POST: Añadir producto ────────────────────────────────────────────────

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarAlCarrito(int productoId, int cantidad = 1)
        {
            var producto = _context.Productos.Find(productoId);
            if (producto == null) return NotFound();

            var carrito = GetCarrito();
            var item = carrito.FirstOrDefault(i => i.ProductoId == productoId);

            if (item != null)
                item.Cantidad += cantidad;
            else
                carrito.Add(new CarritoItem
                {
                    ProductoId = productoId,
                    Nombre     = producto.Nombre,
                    ImagenUrl  = producto.ImagenUrl,
                    Precio     = producto.Precio,
                    Cantidad   = cantidad
                });

            GuardarCarrito(carrito);
            TempData["Mensaje"] = $"✓ '{producto.Nombre}' añadido al carrito.";
            return RedirectToAction("Index");
        }

        // ─── POST: Actualizar cantidad ────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarCantidad(int productoId, int cantidad)
        {
            var carrito = GetCarrito();
            var item = carrito.FirstOrDefault(i => i.ProductoId == productoId);
            if (item != null)
            {
                if (cantidad <= 0)
                    carrito.Remove(item);
                else
                    item.Cantidad = cantidad;
            }
            GuardarCarrito(carrito);
            return RedirectToAction("Index");
        }

        // ─── POST: Eliminar producto ──────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int productoId)
        {
            var carrito = GetCarrito();
            carrito.RemoveAll(i => i.ProductoId == productoId);
            GuardarCarrito(carrito);
            return RedirectToAction("Index");
        }

        // ─── POST: Vaciar carrito ─────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Vaciar()
        {
            GuardarCarrito(new List<CarritoItem>());
            return RedirectToAction("Index");
        }

        // ─── POST: Crear sesión de pago Stripe ────────────────────────────────────

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearSesionPago()
        {
            var carrito = GetCarrito();
            if (!carrito.Any()) return RedirectToAction("Index");

            var domain = $"{Request.Scheme}://{Request.Host}";

            var lineItems = carrito.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency   = "eur",
                    UnitAmount = (long)(item.Precio * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name        = item.Nombre,
                        Description = $"Ref. #{item.ProductoId} — SENPRO"
                    }
                },
                Quantity = item.Cantidad
            }).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems          = lineItems,
                Mode               = "payment",
                SuccessUrl         = $"{domain}/Carrito/Confirmacion?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl          = $"{domain}/Carrito/Index",
                CustomerEmail      = User.FindFirstValue(ClaimTypes.Email)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            // Guardar pedido pendiente en BD
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var pedido = new Pedido
            {
                UsuarioId     = userId,
                FechaPedido   = DateTime.Now,
                Total         = carrito.Sum(i => i.Subtotal),
                Estado        = "Pendiente",
                StripeSessionId = session.Id,
                Lineas        = carrito.Select(i => new LineaPedido
                {
                    ProductoId      = i.ProductoId,
                    Cantidad        = i.Cantidad,
                    PrecioUnitario  = i.Precio,
                    NombreProducto  = i.Nombre
                }).ToList()
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Redirect(session.Url!);
        }

        // ─── GET: Confirmación de pago ────────────────────────────────────────────

        public async Task<IActionResult> Confirmacion(string session_id)
        {
            if (string.IsNullOrEmpty(session_id))
                return RedirectToAction("Index", "Home");

            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                var pedido = _context.Pedidos
                    .FirstOrDefault(p => p.StripeSessionId == session_id);

                if (pedido != null && pedido.Estado != "Pagado")
                {
                    pedido.Estado = "Pagado";
                    await _context.SaveChangesAsync();
                }

                GuardarCarrito(new List<CarritoItem>());
            }

            ViewBag.PaymentStatus = session.PaymentStatus;
            ViewBag.AmountTotal   = (session.AmountTotal ?? 0) / 100.0;
            ViewBag.CustomerEmail = session.CustomerEmail;
            return View();
        }
    }
}
