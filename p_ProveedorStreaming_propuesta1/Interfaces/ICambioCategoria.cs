using System;
using System.Collections.Generic;
using System.Text;
using p_ProveedorStreaming.Clases.strUsuario;

namespace p_ProveedorStreaming.Interfaces
{
    public interface ICambioCategoria
    {
        // Solo recibe al usuario para evaluar sus puntos y actualizar su rango
        void CambiarCategoria(Usuario usuario);
    }
}