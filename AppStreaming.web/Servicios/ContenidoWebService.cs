using p_ProveedorStreaming;
using p_ProveedorStreaming.Clases.strContenido;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strContenido.Clases.strPelicula;
using p_ProveedorStreaming.Clases.strContenido.Clases.strSerie;

namespace AppStreaming.web.Servicios
{
    public class ContenidoWebService
    {
        private readonly ContenidoService _service;
        private NotificacionService? _notif;

        public ContenidoWebService(MensajeFactory factory)
        {
            _service = new ContenidoService(factory);
        }

        public void SuscribirEventos(NotificacionService notif)
        {
            _notif = notif;
        }

        public void AgregarPelicula(string nombre, TimeSpan duracion, byte calificacion)
        {
            var pelicula = new Pelicula(nombre, duracion, calificacion);

            // Suscribir evento NuevoTitulo antes de agregar
            pelicula.pub_nuevo_tit.EventoNuevoTitulo += titulo =>
                _notif?.Agregar($"[Nuevo Título] '{titulo}' ya disponible en el catálogo.");

            // Suscribir evento ContenidoVisto
            pelicula.pub_contenido_visto.EventoContenidoVisto += (nombreContenido, usuario) =>
                _notif?.Agregar($"[Contenido Visto] {usuario.Nombre} terminó de ver '{nombreContenido}'.");

            _service.Agregar(pelicula);
            pelicula.ObtenerNuevoTitulo(pelicula.Nombre);
        }

        public void AgregarSerie(string nombre, byte temporadas, byte capXTemp)
        {
            var serie = new Serie(nombre, temporadas, capXTemp);

            serie.pub_nuevo_tit.EventoNuevoTitulo += titulo =>
                _notif?.Agregar($"[Nuevo Título] '{titulo}' ya disponible en el catálogo.");

            serie.pub_contenido_visto.EventoContenidoVisto += (nombreContenido, usuario) =>
                _notif?.Agregar($"[Contenido Visto] {usuario.Nombre} terminó de ver episodio de '{nombreContenido}'.");

            _service.Agregar(serie);
            serie.ObtenerNuevoTitulo(serie.Nombre);
        }

        public void Cargar(string nomArchivo) => _service.Cargar(nomArchivo);

        public List<Contenido> ObtenerTodos() => _service.ObtenerTodos();

        public Contenido ObtenerPorTitulo(string nombre) => _service.ObtenerPorTitulo(nombre);
    }
}
