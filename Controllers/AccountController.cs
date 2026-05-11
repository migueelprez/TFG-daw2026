using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TFG.Data;
using TFG.Models;

namespace TFG.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var user = _context.Usuarios.FirstOrDefault(u =>
                u.Email == model.Email && u.Activo);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                return View(model);
            }

            await SignInUser(user, model.RememberMe);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return user.Rol == "Admin"
                ? RedirectToAction("Index", "Admin")
                : RedirectToAction("Index", "Home");
        }

        // GET /Account/Register
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.Usuarios.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Ya existe una cuenta con ese email.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Rol = "Usuario",
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            await SignInUser(user, false);
            return RedirectToAction("Index", "Home");
        }

        // GET /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET /Account/AccesoDenegado
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        // ─── MIS PEDIDOS ─────────────────────────────────────────────────────────

        [Authorize]
        public IActionResult MisPedidos()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return RedirectToAction("Login");

            var pedidos = _context.Pedidos
                .Include(p => p.Lineas)
                .Where(p => p.UsuarioId == userId)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return View(pedidos);
        }

        // ─── RECUPERACIÓN DE CONTRASEÑA ──────────────────────────────────────────

        // GET /Account/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("", "Por favor introduce tu email.");
                return View();
            }

            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Activo);

            // Siempre mostramos mensaje de éxito por seguridad
            ViewBag.Enviado = true;

            if (user != null)
            {
                // Invalidar tokens anteriores
                var tokensAnteriores = _context.PasswordResetTokens
                    .Where(t => t.Email == email && !t.Usado);
                _context.PasswordResetTokens.RemoveRange(tokensAnteriores);

                var token = new PasswordResetToken
                {
                    Email = email,
                    Token = Guid.NewGuid().ToString("N"),
                    Expiracion = DateTime.Now.AddHours(2),
                    Usado = false
                };

                _context.PasswordResetTokens.Add(token);
                await _context.SaveChangesAsync();

                // En producción aquí enviarías el email. En desarrollo lo ponemos en ViewBag para debug.
                var resetUrl = Url.Action("ResetPassword", "Account",
                    new { token = token.Token }, Request.Scheme);
                ViewBag.DebugLink = resetUrl; // Solo visible en desarrollo
            }

            return View();
        }

        // GET /Account/ResetPassword
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            var tokenObj = _context.PasswordResetTokens
                .FirstOrDefault(t => t.Token == token && !t.Usado && t.Expiracion > DateTime.Now);

            if (tokenObj == null)
            {
                ViewBag.TokenInvalido = true;
                return View();
            }

            ViewBag.Token = token;
            return View();
        }

        // POST /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string token, string password, string confirmar)
        {
            if (password != confirmar)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                ViewBag.Token = token;
                return View();
            }

            if (password.Length < 6)
            {
                ModelState.AddModelError("", "La contraseña debe tener al menos 6 caracteres.");
                ViewBag.Token = token;
                return View();
            }

            var tokenObj = _context.PasswordResetTokens
                .FirstOrDefault(t => t.Token == token && !t.Usado && t.Expiracion > DateTime.Now);

            if (tokenObj == null)
            {
                ViewBag.TokenInvalido = true;
                return View();
            }

            var user = _context.Usuarios.FirstOrDefault(u => u.Email == tokenObj.Email);
            if (user == null)
            {
                ViewBag.TokenInvalido = true;
                return View();
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            tokenObj.Usado = true;
            await _context.SaveChangesAsync();

            ViewBag.Exito = true;
            return View();
        }

        // ─── Helper ─────────────────────────────────────────────────────────────
        private async Task SignInUser(ApplicationUser user, bool persistent)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name,           user.Nombre),
                new(ClaimTypes.Email,          user.Email),
                new(ClaimTypes.Role,           user.Rol)
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = persistent });
        }
    }
}
