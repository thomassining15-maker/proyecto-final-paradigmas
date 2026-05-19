using p_ProveedorStreaming.Clases.strUsuario;
using System;

namespace p_ProveedorStreaming.Eventos
{
    public class PublisherContenidoVisto
    {
        public delegate void dele_contenido_visto(string nombreContenido, Usuario usuario);
        public event dele_contenido_visto EventoContenidoVisto;

        public string InformarContenidoVisto(string nombreContenido, Usuario usuario)
        {
            EventoContenidoVisto?.Invoke(nombreContenido, usuario);
            return $"'{nombreContenido}' marcado como visto por {usuario.Nombre}";
        }
    }
}
