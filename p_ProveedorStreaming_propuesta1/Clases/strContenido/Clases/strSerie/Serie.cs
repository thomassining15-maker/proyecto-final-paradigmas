using p_ProveedorStreaming.Eventos;
using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Clases.strContenido.Clases.strSerie
{
    internal class Serie : Contenido
    {
        private byte temporadas;
        private byte cap_x_temp;
        private PublisherNuevoTitulo pub_nuevo_tit;

        public Serie(string nombre, byte temporadas, byte cap_x_temp) : base(nombre)
        {
            this.Temporadas = temporadas;
            this.Cap_x_temp = cap_x_temp;
        }

        public byte Temporadas { get => temporadas; set => temporadas = value >= ReglasNegocioContenido.min_temps_serie ? 
                value : throw new Exception($"El minimo de temporadas por serie es {ReglasNegocioContenido.min_temps_serie}"); }

        public byte Cap_x_temp { get => cap_x_temp; set => cap_x_temp = value >= ReglasNegocioContenido.min_caps_x_temp ? 
                value : throw new Exception($"El numero minimo de capitulos por temporaada es {ReglasNegocioContenido.min_caps_x_temp}"); }
    }
}
