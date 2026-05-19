namespace AppStreaming.web.Servicios
{
    public class NotificacionService
    {
        private readonly List<string> _mensajes = new();

        public void Agregar(string mensaje)
        {
            _mensajes.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {mensaje}");
            if (_mensajes.Count > 50) _mensajes.RemoveAt(_mensajes.Count - 1);
        }

        public IReadOnlyList<string> ObtenerTodos() => _mensajes.AsReadOnly();
    }
}
