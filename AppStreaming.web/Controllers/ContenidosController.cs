using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class ContenidosController : Controller
{
    private readonly ContenidoWebService _contenidoSvc;
    private readonly NotificacionService _notif;

    public ContenidosController(ContenidoWebService contenidoSvc, NotificacionService notif)
    {
        _contenidoSvc = contenidoSvc;
        _notif = notif;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        return View(_contenidoSvc.ObtenerTodos());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");
        return View();
    }

    [HttpPost]
    public IActionResult Crear(string tipo, string nombre,
        string? duracion, byte? calificacion, byte? temporadas, byte? capXTemp)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");

        try
        {
            if (tipo == "P")
            {
                var dur = TimeSpan.Parse(duracion ?? "01:30:00");
                _contenidoSvc.AgregarPelicula(nombre, dur, calificacion ?? 1);
            }
            else
            {
                _contenidoSvc.AgregarSerie(nombre, temporadas ?? 1, capXTemp ?? 5);
            }

            TempData["Exito"] = $"'{nombre}' agregado. El evento NuevoTítulo fue disparado.";
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }

        return RedirectToAction("Index");
    }
}
