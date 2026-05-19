using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class JuegosController : Controller
{
    private readonly JuegoWebService _juegoSvc;
    private readonly UsuarioWebService _usuarioSvc;

    public JuegosController(JuegoWebService juegoSvc, UsuarioWebService usuarioSvc)
    {
        _juegoSvc = juegoSvc;
        _usuarioSvc = usuarioSvc;
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
            _juegoSvc.Agregar(nombre, genero);
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
            var usuario = _usuarioSvc.BuscarPorNombre(nombreUsuario);
            string msg = _juegoSvc.RegistrarPuntaje(nombreJuego, usuario, puesto);
            TempData["Exito"] = msg;
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}
