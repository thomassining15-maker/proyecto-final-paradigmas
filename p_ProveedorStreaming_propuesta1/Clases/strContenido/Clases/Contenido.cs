using p_ProveedorStreaming.Interfaces;
using p_ProveedorStreaming.Eventos;
using System;

namespace p_ProveedorStreaming.Clases.strContenido.Clases
{
    // Implementa IActualizacionPuntos para permitir la interceptación
    public abstract class Contenido : IActualizacionPuntos
    {
        private string nombre;
        public PublisherNuevoTitulo pub_nuevo_tit = new();

        public Contenido(string nombre)
        {
            this.nombre = nombre;
        }

        public string Nombre => nombre;

        // Marcamos como virtual para que el Interceptor_GestionPuntos haga su magia
        public virtual void Reproducir()
        {
            Console.WriteLine($"[Reproduciendo] {nombre}...");
            // Aquí el interceptor detecta la reproducción y llama a ActualizarPuntaje
        }

        // Este método será invocado por el interceptor Interceptor_GestionPuntos
        public virtual void ActualizarPuntaje()
        {
            // La lógica de puntos no va aquí, la maneja el Interceptor usando ReglaNegocioContenido
            Console.WriteLine($"[Sistema] Interceptando para actualizar puntaje de: {nombre}");
        }

        public void ObtenerNuevoTitulo(object titulo)
        {
            pub_nuevo_tit.InformarNuevoTitulo(titulo);
        }

        public void EventHandler(object titulo)
        {
            Console.WriteLine($"[Notificación] Nuevo título disponible: {titulo}");
        }
    }
}