using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strUsuario;

namespace AppStreaming.web.Controllers;

public class CuentasController : Controller
{
    private readonly CuentaService _cuentaSvc;
    private readonly UsuarioService _usuarioSvc;

    public CuentasController(CuentaService cuentaSvc, UsuarioService usuarioSvc)
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
            var usuario = _usuarioSvc.BuscarPorNombre(nombreUsuario);
            _cuentaSvc.Crear(usuario);
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
