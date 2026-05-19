using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Models;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class HomeController : Controller
{
    private readonly NotificacionService _notif;
    private readonly UsuarioWebService _usuarioSvc;
    private readonly ContenidoWebService _contenidoSvc;
    private readonly JuegoWebService _juegoSvc;

    public HomeController(NotificacionService notif, UsuarioWebService usuarioSvc,
        ContenidoWebService contenidoSvc, JuegoWebService juegoSvc)
    {
        _notif = notif;
        _usuarioSvc = usuarioSvc;
        _contenidoSvc = contenidoSvc;
        _juegoSvc = juegoSvc;
    }

    public IActionResult Index()
    {
        ViewBag.Notificaciones = _notif.ObtenerTodos();
        ViewBag.TotalUsuarios = _usuarioSvc.ObtenerTodos().Count;
        ViewBag.TotalContenidos = _contenidoSvc.ObtenerTodos().Count;
        ViewBag.TotalJuegos = _juegoSvc.ObtenerTodos().Count;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
