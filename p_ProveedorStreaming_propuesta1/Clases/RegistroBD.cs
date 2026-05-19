using p_ProveedorStreaming.Interfaces;
using System;

namespace p_ProveedorStreaming.Clases
{
    public class RegistroBD : IRegistroEntidad
    {
        public string Registrar(object entidad)
        {
            return $"Entidad '{entidad}' persistida en BD.";
        }
    }
}
