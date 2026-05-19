using p_ProveedorStreaming.Interfaces;
using System;

namespace p_ProveedorStreaming.Clases
{
    public class AccesoService
    {
        private readonly MensajeFactory _factory;

        public AccesoService(MensajeFactory factory)
        {
            _factory = factory;
        }

        public (bool ok, string mensaje, string rol) Login(string usuario, string clave)
        {
            IAcceso proxy = _factory.CrearMensajeAutenticacion(new AccesoSistema());
            try
            {
                bool resultado = proxy.IngresarSistema(usuario, clave);
                if (resultado)
                {
                    string rol = usuario == "admin" ? "admin" : "usuario";
                    return (true, "Acceso concedido.", rol);
                }
                return (false, "Credenciales incorrectas.", "");
            }
            catch (Exception ex)
            {
                return (false, ex.Message, "");
            }
        }
    }
}
