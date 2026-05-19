using Microsoft.AspNetCore.Mvc;
using AppStreaming.web.Servicios;

namespace AppStreaming.web.Controllers;

public class ReproduccionController : Controller
{
    private readonly ContenidoWebService _contenidoSvc;
    private readonly UsuarioWebService _usuarioSvc;
    private readonly CuentaWebService _cuentaSvc;

    public ReproduccionController(ContenidoWebService contenidoSvc,
        UsuarioWebService usuarioSvc, CuentaWebService cuentaSvc)
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
            var usuario = _usuarioSvc.BuscarPorNombre(nombreUsuario);
            var contenido = _contenidoSvc.ObtenerPorTitulo(nombreContenido);

            // Asignar usuario activo y reproducir — el aspecto GestionPuntos interviene aquí
            contenido.UsuarioActivo = usuario;
            contenido.Reproducir();

            // Registrar en la cuenta del usuario
            _cuentaSvc.RegistrarVisualizacion((ulong)usuario.Id_interno, contenido);

            TempData["Exito"] = $"{usuario.Nombre} reprodujo '{nombreContenido}'. Puntos actuales: {usuario.Puntos} | Categoría: {usuario.Categoria}";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}
