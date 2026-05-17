using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Clases.strContenido.Clases
{
    public static class ReglasNegocioContenido
    {
        public static readonly byte calificacion_min = 1;
        public static readonly byte calificacion_max = 2;
        public static readonly byte lon_min_nombre = 1;
        public static readonly byte min_temps_serie = 1;
        public static readonly byte min_caps_x_temp = 5;
        //Una Pelicula corta es la que dura menos de 1 hora y 30
        public static readonly byte pts_pelicula_corta = 50;
        //Un episodio corto es aquel que dura menos de 45 mins
        public static readonly byte pts_episodio_corto = 28;
        //Una pelicula larga es aquella que dura más de 1 hora y 30
        public static readonly byte pts_pelicula_larga = 50;
        //Un episodio corto es aquel que dura más de 45 mins
        public static readonly byte pts_episodio_largo = 28;
    }
}
