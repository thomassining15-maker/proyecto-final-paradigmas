using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Models;
using p_ProveedorStreaming.Clases;

namespace AppStreaming.web.Controllers;

public class AuthController : Controller
{
    private readonly AccesoService _accesoService;

    public AuthController(AccesoService accesoService)
    {
        _accesoService = accesoService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("usuario") != null)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (ok, mensaje, rol) = _accesoService.Login(model.Usuario, model.Clave);

        if (ok)
        {
            HttpContext.Session.SetString("usuario", model.Usuario);
            HttpContext.Session.SetString("rol", rol);
            TempData["Exito"] = $"Bienvenido, {model.Usuario}!";
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = mensaje;
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
