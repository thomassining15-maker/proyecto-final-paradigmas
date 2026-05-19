using p_ProveedorStreaming.Interfaces;

namespace AppStreaming.web.Servicios
{
    public class NotificacionService : INotificador
    {
        private readonly List<string> _mensajes = new();

        public void Agregar(string mensaje)
        {
            _mensajes.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {mensaje}");
            if (_mensajes.Count > 50) _mensajes.RemoveAt(_mensajes.Count - 1);
        }

        public void NotificarNuevoTitulo(string titulo) =>
            Agregar($"[Nuevo Título] '{titulo}' ya disponible en el catálogo.");

        public void NotificarContenidoVisto(string usuario, string contenido) =>
            Agregar($"[Contenido Visto] {usuario} terminó de ver '{contenido}'.");

        public void NotificarNuevoRecord(string juego, ulong record) =>
            Agregar($"[Nuevo Récord] ¡Récord de {record} pts registrado en '{juego}'!");

        public IReadOnlyList<string> ObtenerTodos() => _mensajes.AsReadOnly();
    }
}
