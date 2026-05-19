using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strUsuario;

namespace AppStreaming.web.Controllers;

public class JuegosController : Controller
{
    private readonly JuegoService _juegoSvc;
    private readonly UsuarioService _usuarioSvc;
    private readonly NotificacionService _notif;

    public JuegosController(JuegoService juegoSvc, UsuarioService usuarioSvc, NotificacionService notif)
    {
        _juegoSvc = juegoSvc;
        _usuarioSvc = usuarioSvc;
        _notif = notif;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        return View(_juegoSvc.ObtenerTodos());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");
        return View();
    }

    [HttpPost]
    public IActionResult Crear(string nombre, string genero)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");

        try
        {
            _juegoSvc.Agregar(nombre, genero, _notif);
            TempData["Exito"] = $"Juego '{nombre}' agregado al catálogo.";
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Record(string nombre)
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        ViewBag.NombreJuego = nombre;
        ViewBag.Usuarios = _usuarioSvc.ObtenerTodos();
        return View();
    }

    [HttpPost]
    public IActionResult Record(string nombreJuego, string nombreUsuario, byte puesto)
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        try
        {
            string msg = _juegoSvc.RegistrarPuntaje(nombreJuego, nombreUsuario, puesto, _usuarioSvc);
            TempData["Exito"] = msg;
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}
