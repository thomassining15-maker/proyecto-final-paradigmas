using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Clases.strContenido.Clases;

namespace AppStreaming.web.Servicios
{
    public class CuentaWebService
    {
        private readonly CuentaService _service = new();
        private readonly UsuarioWebService _usuarioSvc;

        public CuentaWebService(UsuarioWebService usuarioSvc)
        {
            _usuarioSvc = usuarioSvc;
        }

        public void Crear(string nombreUsuario)
        {
            var usuario = _usuarioSvc.BuscarPorNombre(nombreUsuario);
            _service.Crear(usuario);
        }

        public void Cargar(string nomArchivo) => _service.Cargar(nomArchivo, _usuarioSvc._service);

        public List<Cuenta> ObtenerTodas() => _service.ObtenerTodas();

        public Cuenta? ObtenerCuentaPorUsuario(ulong idUsuario) => _service.ObtenerCuentaPorUsuario(idUsuario);

        public void RegistrarVisualizacion(ulong idUsuario, Contenido contenido)
        {
            _service.RegistrarVisualizacion(idUsuario, contenido);
        }
    }
}
