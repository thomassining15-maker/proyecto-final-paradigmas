using System;
using System.Collections.Generic;
using System.Text;
using p_ProveedorStreaming.Clases.strUsuario;
using static p_ProveedorStreaming.Clases.strUsuario.ReglasNegocioUsuario;

namespace p_ProveedorStreaming.Eventos
{
    public class PublisherCambioCategoria
    {
        public delegate void dele_cambio_cat(Usuario usuario);
        public event dele_cambio_cat EventoCambioCategoria;

        public string InformarCambioCategoria(Usuario usuario, l_categorias nueva_cat)
        {
            EventoCambioCategoria?.Invoke(usuario);
            return $"El usuario {usuario.Nombre} ahora es categoría {nueva_cat}";
        }
    }
}
