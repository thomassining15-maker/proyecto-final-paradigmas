using p_ProveedorStreaming.Eventos;
using System;

namespace p_ProveedorStreaming.Clases.strContenido.Clases.strSerie
{
    internal class Serie : Contenido
    {
        private byte temporadas;
        private byte cap_x_temp;

        public Serie(string nombre, byte temporadas, byte cap_x_temp) : base(nombre)
        {
            this.Temporadas = temporadas;
            this.Cap_x_temp = cap_x_temp;
        }

        public byte Temporadas
        {
            get => temporadas;
            set => temporadas = value >= ReglasNegocioContenido.min_temps_serie
                ? value
                : throw new Exception($"Mínimo de temporadas por serie: {ReglasNegocioContenido.min_temps_serie}");
        }

        public byte Cap_x_temp
        {
            get => cap_x_temp;
            set => cap_x_temp = value >= ReglasNegocioContenido.min_caps_x_temp
                ? value
                : throw new Exception($"Mínimo de capítulos por temporada: {ReglasNegocioContenido.min_caps_x_temp}");
        }

        public override void ActualizarPuntaje()
        {
            // Otorga puntos por cada episodio visto (un episodio al llamar Reproducir)
            UsuarioActivo?.SumarPuntos(ReglasNegocioContenido.pts_episodio_corto);
            UsuarioActivo?.CambiarCategoria(UsuarioActivo);
            Console.WriteLine($"[Puntaje] +{ReglasNegocioContenido.pts_episodio_corto} pts a {UsuarioActivo?.Nombre} por ver episodio de '{Nombre}'");
        }
    }
}
