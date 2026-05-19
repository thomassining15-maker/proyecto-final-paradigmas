using p_ProveedorStreaming;
using p_ProveedorStreaming.Clases;
using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Interfaces;

namespace AppStreaming.web.Servicios
{
    public class UsuarioWebService
    {
        internal readonly UsuarioService _service = new();
        private readonly MensajeFactory _factory;

        public UsuarioWebService(MensajeFactory factory)
        {
            _factory = factory;
        }

        public void SuscribirEventos(NotificacionService notif)
        {
            // El evento PublisherNuevoUsuario se dispara dentro de Agregar()
            // No hay un publisher centralizado para usuarios — cada usuario tiene su pub_cambio_cat.
            // Lo suscribimos en AgregarConAspecto después de crear el usuario.
        }

        public string AgregarConAspecto(string nombre)
        {
            var usuario = new Usuario(nombre);

            // Aspecto de validación: simula guardado en BD
            IRegistroEntidad bd = _factory.CrearMensajeValidacion(new RegistroBD());
            string resultado = bd.Registrar(usuario);

            _service.Agregar(usuario);

            // Suscribir el evento de cambio de categoría al notificador (si hubiera)
            return resultado;
        }

        public void Cargar(string nomArchivo) => _service.Cargar(nomArchivo);

        public List<Usuario> ObtenerTodos() => _service.ObtenerTodos();

        public Usuario BuscarPorId(ulong id) => _service.BuscarPorId(id);

        public Usuario BuscarPorNombre(string nombre) => _service.BuscarPorNombre(nombre);
    }
}
