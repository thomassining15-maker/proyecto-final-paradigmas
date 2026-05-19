using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Models;
using AppStreaming.web.Servicios;
using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strUsuario;

namespace AppStreaming.web.Controllers;

public class HomeController : Controller
{
    private readonly NotificacionService _notif;
    private readonly UsuarioService _usuarioSvc;
    private readonly ContenidoService _contenidoSvc;
    private readonly JuegoService _juegoSvc;

    public HomeController(NotificacionService notif, UsuarioService usuarioSvc,
        ContenidoService contenidoSvc, JuegoService juegoSvc)
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
