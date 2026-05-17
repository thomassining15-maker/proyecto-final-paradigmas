using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Eventos
{
    public class PublisherNuevoTitulo
    {
        // Firma: +dele_nuevo_tit (object titulo) : void delegate
        public delegate void dele_nuevo_tit(object titulo);

        // Evento: +dele_nuevo_tit : event dele_nuevo_tit
        public event dele_nuevo_tit EventoNuevoTitulo;

        // Método: +InformarNuevoTitulo (titulo:object) : string
        public string InformarNuevoTitulo(object titulo)
        {
            EventoNuevoTitulo?.Invoke(titulo);
            return $"Notificación enviada para: {titulo}";
        }
    }
}