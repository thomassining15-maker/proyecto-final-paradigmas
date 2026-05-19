using p_ProveedorStreaming.Interfaces;
using System;

namespace p_ProveedorStreaming.Clases
{
    public class AccesoSistema : IAcceso
    {
        // Credenciales válidas para la demo (en un sistema real vendrían de BD)
        private static readonly string USUARIO_ADMIN = "admin";
        private static readonly string CLAVE_ADMIN   = "1234";

        public bool IngresarSistema(string usuario, string clave)
        {
            return usuario == USUARIO_ADMIN && clave == CLAVE_ADMIN;
        }
    }
}
