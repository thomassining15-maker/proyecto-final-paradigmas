using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;
using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strUsuario;

namespace AppStreaming.web.Controllers;

public class ReproduccionController : Controller
{
    private readonly ContenidoService _contenidoSvc;
    private readonly UsuarioService _usuarioSvc;
    private readonly CuentaService _cuentaSvc;

    public ReproduccionController(ContenidoService contenidoSvc,
        UsuarioService usuarioSvc, CuentaService cuentaSvc)
    {
        _contenidoSvc = contenidoSvc;
        _usuarioSvc = usuarioSvc;
        _cuentaSvc = cuentaSvc;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        ViewBag.Usuarios = _usuarioSvc.ObtenerTodos();
        ViewBag.Contenidos = _contenidoSvc.ObtenerTodos();
        return View();
    }

    [HttpPost]
    public IActionResult Reproducir(string nombreUsuario, string nombreContenido)
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        try
        {
            string msg = _contenidoSvc.Reproducir(nombreUsuario, nombreContenido, _usuarioSvc, _cuentaSvc);
            TempData["Exito"] = msg;
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}
