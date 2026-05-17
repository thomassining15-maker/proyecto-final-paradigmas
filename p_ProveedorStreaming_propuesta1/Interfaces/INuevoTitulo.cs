using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Interfaces
{
    public interface INuevoTitulo
    {
        // Recibe el objeto (título) y el notificador que gestiona la acción
        void ObtenerNuevoTitulo(object titulo, INuevoTitulo notificador);
    }
}