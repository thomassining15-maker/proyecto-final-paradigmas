using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class CuentasController : Controller
{
    private readonly CuentaWebService _cuentaSvc;
    private readonly UsuarioWebService _usuarioSvc;

    public CuentasController(CuentaWebService cuentaSvc, UsuarioWebService usuarioSvc)
    {
        _cuentaSvc = cuentaSvc;
        _usuarioSvc = usuarioSvc;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("usuario") == null)
            return RedirectToAction("Login", "Auth");

        return View(_cuentaSvc.ObtenerTodas());
    }

    [HttpGet]
    public IActionResult Crear()
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");

        ViewBag.Usuarios = _usuarioSvc.ObtenerTodos();
        return View();
    }

    [HttpPost]
    public IActionResult Crear(string nombreUsuario)
    {
        if (HttpContext.Session.GetString("rol") != "admin")
            return RedirectToAction("Index");

        try
        {
            _cuentaSvc.Crear(nombreUsuario);
            TempData["Exito"] = $"Cuenta creada para {nombreUsuario}.";
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            ViewBag.Usuarios = _usuarioSvc.ObtenerTodos();
            return View();
        }

        return RedirectToAction("Index");
    }
}
