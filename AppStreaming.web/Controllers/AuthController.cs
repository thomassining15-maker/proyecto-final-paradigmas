using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Models;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
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

        var (ok, mensaje) = _authService.Login(model.Usuario, model.Clave);

        if (ok)
        {
            HttpContext.Session.SetString("usuario", model.Usuario);
            HttpContext.Session.SetString("rol", model.Usuario == "admin" ? "admin" : "usuario");
            TempData["Exito"] = $"Bienvenido, {model.Usuario}!";
            return RedirectToAction("Index", "Home");
        }

        // El aspecto de autenticación registró el intento; mostramos el error
        ViewBag.Error = mensaje;
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
