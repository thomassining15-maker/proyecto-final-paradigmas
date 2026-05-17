using p_ProveedorStreaming.Eventos;
using System;

namespace p_ProveedorStreaming.Clases.strContenido.Clases.strPelicula
{
    internal class Pelicula : Contenido
    {
        private TimeSpan duracion;
        private byte calificacion;

        public Pelicula(string nombre, TimeSpan duracion, byte calificacion) : base(nombre)
        {
            this.duracion = duracion;
            Calificacion = calificacion;
        }

        public TimeSpan Duracion => duracion;
        public byte Calificacion
        {
            get => calificacion;
            set => calificacion = value >= ReglasNegocioContenido.calificacion_min && value <= ReglasNegocioContenido.calificacion_max
                ? value
                : throw new Exception($"Calificación inválida: debe estar entre {ReglasNegocioContenido.calificacion_min} y {ReglasNegocioContenido.calificacion_max}");
        }

        public override void ActualizarPuntaje()
        {
            int puntos = duracion < new TimeSpan(1, 30, 0)
                ? ReglasNegocioContenido.pts_pelicula_corta
                : ReglasNegocioContenido.pts_pelicula_larga;

            UsuarioActivo?.SumarPuntos(puntos);
            UsuarioActivo?.CambiarCategoria(UsuarioActivo);
            Console.WriteLine($"[Puntaje] +{puntos} pts a {UsuarioActivo?.Nombre} por ver '{Nombre}'");
        }
    }
}
