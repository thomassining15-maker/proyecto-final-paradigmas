namespace p_ProveedorStreaming.Interfaces
{
    public interface IAcceso
    {
        // Verifica credenciales de acceso al sistema
        bool IngresarSistema(string usuario, string clave);
    }
}
