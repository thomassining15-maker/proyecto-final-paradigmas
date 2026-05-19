using AppStreaming.web.Servicios;
using p_ProveedorStreaming.Aspectos;
using p_ProveedorStreaming;

var builder = WebApplication.CreateBuilder(args);

// ─── MVC ───────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ─── SESIÓN (aspecto de autenticación) ─────────────────────────────────────
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ─── ASPECTOS ──────────────────────────────────────────────────────────────
builder.Services.AddSingleton<Interceptor_GestionPuntos>();
builder.Services.AddSingleton<Interceptor_GestionRecord>();
builder.Services.AddSingleton<Interceptor_Validacion>();
builder.Services.AddSingleton<Interceptor_Autenticacion>();
builder.Services.AddSingleton<MensajeFactory>();

// ─── SERVICIOS WEB (Singleton = estado compartido en toda la app)
builder.Services.AddSingleton<UsuarioWebService>();
builder.Services.AddSingleton<ContenidoWebService>();
builder.Services.AddSingleton<JuegoWebService>();
builder.Services.AddSingleton<CuentaWebService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<NotificacionService>();

var app = builder.Build();

// ─── PRECARGA DE DATOS ─────────────────────────────────────────────────────
var usuarioSvc   = app.Services.GetRequiredService<UsuarioWebService>();
var contenidoSvc = app.Services.GetRequiredService<ContenidoWebService>();
var juegoSvc     = app.Services.GetRequiredService<JuegoWebService>();
var cuentaSvc    = app.Services.GetRequiredService<CuentaWebService>();
var notifSvc     = app.Services.GetRequiredService<NotificacionService>();

// Suscribir los 4 eventos a la bandeja de notificaciones antes de cargar datos
usuarioSvc.SuscribirEventos(notifSvc);
contenidoSvc.SuscribirEventos(notifSvc);
juegoSvc.SuscribirEventos(notifSvc);

// Cargar datos desde archivos .txt
try { usuarioSvc.Cargar("Usuarios.txt"); } catch { }
try { contenidoSvc.Cargar("Contenidos.txt"); } catch { }
try { juegoSvc.Cargar("Juegos.txt"); } catch { }
try { cuentaSvc.Cargar("Cuentas.txt"); } catch { }

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
