using p_ProveedorStreaming.Eventos;
using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Clases.strContenido.Clases.strPelicula
{
    internal class Pelicula : Contenido
    {
        private TimeSpan duracion;
        private byte calificacion;
        private PublisherNuevoTitulo pub_nuevo_tit;

        public Pelicula(string nombre, TimeSpan duracion, byte calificacion) : base(nombre)
        {
            this.duracion = duracion;
            Calificacion = calificacion;
        }

        public TimeSpan Duracion { get => duracion;}
        public byte Calificacion { get => calificacion; set => calificacion =
                value >= ReglasNegocioContenido.calificacion_min && value <= ReglasNegocioContenido.calificacion_max ? 
                value : throw new Exception($"La calificacion para la pelicula no es válida," +
                    $" debe de estar entre {ReglasNegocioContenido.calificacion_min} y {ReglasNegocioContenido.calificacion_max}"); }
    }
}
