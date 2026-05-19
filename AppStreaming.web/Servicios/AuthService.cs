using p_ProveedorStreaming;
using p_ProveedorStreaming.Clases;
using p_ProveedorStreaming.Interfaces;

namespace AppStreaming.web.Servicios
{
    public class AuthService
    {
        private readonly MensajeFactory _factory;

        public AuthService(MensajeFactory factory)
        {
            _factory = factory;
        }

        // Usa el aspecto de autenticación para validar y registrar el intento
        public (bool ok, string mensaje) Login(string usuario, string clave)
        {
            IAcceso proxy = _factory.CrearMensajeAutenticacion(new AccesoSistema());
            try
            {
                bool resultado = proxy.IngresarSistema(usuario, clave);
                return resultado
                    ? (true, "Acceso concedido.")
                    : (false, "Credenciales incorrectas.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
