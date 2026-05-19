using AppStreaming.web.Servicios;
using p_ProveedorStreaming;
using p_ProveedorStreaming.Aspectos;
using p_ProveedorStreaming.Clases;
using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ─── MVC ───────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ─── SESIÓN ────────────────────────────────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ─── ASPECTOS (biblioteca) ─────────────────────────────────────────────────
builder.Services.AddSingleton<Interceptor_GestionPuntos>();
builder.Services.AddSingleton<Interceptor_GestionRecord>();
builder.Services.AddSingleton<Interceptor_Validacion>();
builder.Services.AddSingleton<Interceptor_Autenticacion>();
builder.Services.AddSingleton<MensajeFactory>();

// ─── SERVICIOS DE LA BIBLIOTECA ────────────────────────────────────────────
builder.Services.AddSingleton<UsuarioService>(sp =>
    new UsuarioService(sp.GetRequiredService<MensajeFactory>()));
builder.Services.AddSingleton<ContenidoService>();
builder.Services.AddSingleton<JuegoService>();
builder.Services.AddSingleton<CuentaService>();
builder.Services.AddSingleton<AccesoService>();

// ─── SERVICIO DE NOTIFICACIONES (UI) ───────────────────────────────────────
builder.Services.AddSingleton<NotificacionService>();
builder.Services.AddSingleton<INotificador>(sp => sp.GetRequiredService<NotificacionService>());

var app = builder.Build();

// ─── PRECARGA DE DATOS (bootstrap de la biblioteca) ───────────────────────
var usuarioSvc   = app.Services.GetRequiredService<UsuarioService>();
var contenidoSvc = app.Services.GetRequiredService<ContenidoService>();
var juegoSvc     = app.Services.GetRequiredService<JuegoService>();
var cuentaSvc    = app.Services.GetRequiredService<CuentaService>();

BootstrapDatos.Inicializar(usuarioSvc, contenidoSvc, juegoSvc, cuentaSvc);

// ─── PIPELINE ──────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
