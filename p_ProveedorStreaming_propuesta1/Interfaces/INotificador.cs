namespace p_ProveedorStreaming.Interfaces
{
    public interface INotificador
    {
        void NotificarNuevoTitulo(string titulo);
        void NotificarContenidoVisto(string usuario, string contenido);
        void NotificarNuevoRecord(string juego, ulong record);
    }
}
