using p_ProveedorStreaming;
using p_ProveedorStreaming.Clases.strJuego;
using p_ProveedorStreaming.Clases.strJuego.Clases;
using p_ProveedorStreaming.Clases.strUsuario;

namespace AppStreaming.web.Servicios
{
    public class JuegoWebService
    {
        private readonly JuegoService _service;
        private NotificacionService? _notif;

        public JuegoWebService(MensajeFactory factory)
        {
            _service = new JuegoService(factory);
        }

        public void SuscribirEventos(NotificacionService notif)
        {
            _notif = notif;
        }

        public void Agregar(string nombre, string genero)
        {
            var juego = new Juego(nombre, genero);

            juego.pub_nuevo_tit.EventoNuevoTitulo += titulo =>
                _notif?.Agregar($"[Nuevo Juego] '{titulo}' agregado al catálogo.");

            juego.pub_nuevo_rec.EventoNuevoRecord += record =>
                _notif?.Agregar($"[Nuevo Récord] ¡Récord de {record} pts registrado en '{nombre}'!");

            _service.Agregar(juego);
        }

        public void Cargar(string nomArchivo) => _service.Cargar(nomArchivo);

        public List<Juego> ObtenerTodos() => _service.ObtenerTodos();

        public Juego BuscarPorNombre(string nombre) => _service.BuscarPorNombre(nombre);

        public string RegistrarPuntaje(string nombreJuego, Usuario usuario, byte puesto)
        {
            var juego = _service.BuscarPorNombre(nombreJuego);
            string msg = juego.RegistrarPuntaje(usuario, puesto);

            // Intentar batir el récord del juego
            if (juego.Nro_record == 0 || (puesto == 1 && usuario.Puntos > juego.Nro_record))
                juego.ObtenerNuevoRecord(usuario.Puntos, usuario, juego, juego);

            return msg;
        }
    }
}
