using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TFG.Models;


namespace TFG.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    ///-----------------------------Acción para mostrar la página de contacto--------------------------------------//
    public IActionResult Contacto()
    {
        return View();
    }

    // 2. Acción para recibir el formulario
    [HttpPost]
    public IActionResult Contacto(ContactoModel modelo)
    {
        if (ModelState.IsValid)
        {
            // Aquí es donde en el futuro enviarías un email o guardarías en BD
            ViewBag.MensajeExito = "¡Gracias " + modelo.Nombre + "! Hemos recibido tu mensaje correctamente.";
            return View();
        }

        return View(modelo);
    }
}


