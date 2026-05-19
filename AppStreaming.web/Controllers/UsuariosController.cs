using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class UsuariosController : Controller
{
    private readonly UsuarioWebService _usuarioSvc;
    private readonly NotificacionService _notif;

    public UsuariosController(UsuarioWebService usuarioSvc, NotificacionService notif)
    {
        _usuarioSvc = usuarioSvc;
        _notif = notif;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        return View(_usuarioSvc.ObtenerTodos());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");
        return View();
    }

    [HttpPost]
    public IActionResult Crear(string nombre)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");

        try
        {
            string resultado = _usuarioSvc.AgregarConAspecto(nombre);
            _notif.Agregar($"[Nuevo Usuario] {nombre} registrado en el sistema.");
            TempData["Exito"] = resultado;
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }

        return RedirectToAction("Index");
    }
}
