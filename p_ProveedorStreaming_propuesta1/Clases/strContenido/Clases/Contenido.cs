using p_ProveedorStreaming.Interfaces;
using p_ProveedorStreaming.Eventos;
using p_ProveedorStreaming.Clases.strUsuario;
using System;

namespace p_ProveedorStreaming.Clases.strContenido.Clases
{
    public abstract class Contenido : IActualizacionPuntos
    {
        private string nombre;
        public PublisherNuevoTitulo pub_nuevo_tit = new();
        public PublisherContenidoVisto pub_contenido_visto = new();

        // Usuario que está reproduciendo este contenido — se debe asignar antes de Reproducir()
        public Usuario? UsuarioActivo { get; set; }

        // Requerido por Castle.DynamicProxy
        protected Contenido() : this(string.Empty) { }

        public Contenido(string nombre)
        {
            this.nombre = nombre;
            pub_contenido_visto.EventoContenidoVisto += EventHandlerContenidoVisto;
        }

        public virtual string Nombre => nombre;

        // Virtual para que el proxy de Castle.DynamicProxy pueda interceptarlo
        public virtual void Reproducir()
        {
            Console.WriteLine($"[Reproduciendo] {nombre}...");
        }

        // Invocado por Interceptor_GestionPuntos tras Reproducir()
        // Cada subclase sobreescribe para sumar sus puntos específicos
        public virtual void ActualizarPuntaje()
        {
            Console.WriteLine($"[Puntaje] Sin puntos definidos para: {nombre}");
        }

        // Dispara el cuarto evento: contenido visto
        protected void NotificarContenidoVisto()
        {
            if (UsuarioActivo != null)
                pub_contenido_visto.InformarContenidoVisto(nombre, UsuarioActivo);
        }

        public void EventHandlerContenidoVisto(string nombreContenido, Usuario usuario)
        {
            Console.WriteLine($"[Historial] {usuario.Nombre} terminó de ver '{nombreContenido}'");
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