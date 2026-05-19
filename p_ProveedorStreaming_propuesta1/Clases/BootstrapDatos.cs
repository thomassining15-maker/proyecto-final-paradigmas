using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strUsuario;

namespace p_ProveedorStreaming.Clases
{
    public static class BootstrapDatos
    {
        public static void Inicializar(UsuarioService usuarioSvc, ContenidoService contenidoSvc,
            JuegoService juegoSvc, CuentaService cuentaSvc)
        {
            try { usuarioSvc.Cargar("Usuarios.txt"); } catch { }
            try { contenidoSvc.Cargar("Contenidos.txt"); } catch { }
            try { juegoSvc.Cargar("Juegos.txt"); } catch { }
            try { cuentaSvc.Cargar("Cuentas.txt", usuarioSvc); } catch { }
        }
    }
}
