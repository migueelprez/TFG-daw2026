using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TFG.Data;
using TFG.Models;


namespace TFG.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult QuienesSomos()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // GET /Home/Contacto
    public IActionResult Contacto()
    {
        return View();
    }

    // POST /Home/Contacto
    [HttpPost]
    public IActionResult Contacto(ContactoModel modelo)
    {
        if (ModelState.IsValid)
        {
            ViewBag.MensajeExito = "¡Gracias " + modelo.Nombre + "! Hemos recibido tu mensaje correctamente.";
            return View();
        }

        return View(modelo);
    }

    // GET /Home/Presupuesto
    public IActionResult Presupuesto()
    {
        return View();
    }

    // POST /Home/Presupuesto
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Presupuesto(SolicitudPresupuesto model)
    {
        if (!ModelState.IsValid) return View(model);

        model.FechaSolicitud = DateTime.Now;
        model.Estado = "Pendiente";
        _context.SolicitudesPresupuesto.Add(model);
        await _context.SaveChangesAsync();

        ViewBag.Exito = true;
        return View(new SolicitudPresupuesto());
    }
}
