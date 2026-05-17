using Castle.DynamicProxy;
using p_ProveedorStreaming.Interfaces;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strJuego.Clases;
using p_ProveedorStreaming.Aspectos;

namespace p_ProveedorStreaming
{
    public class MensajeFactory
    {
        private readonly ProxyGenerator _proxyGenerator;
        private readonly Interceptor_GestionPuntos _interceptorGP;
        private readonly Interceptor_GestionRecord _interceptorGR;

        public MensajeFactory(Interceptor_GestionPuntos interceptorPuntos, Interceptor_GestionRecord interceptorRecord)
        {
            _proxyGenerator = new ProxyGenerator();
            _interceptorGP = interceptorPuntos;
            _interceptorGR = interceptorRecord;
        }

        public IActualizacionPuntos CrearMensajeGestionP(object target)
        {
            return _proxyGenerator.CreateClassProxyWithTarget(
                target.GetType(),
                target,
                _interceptorGP) as IActualizacionPuntos;
        }

        public INuevoRecord CrearMensajeGestionR(Juego target)
        {
            return _proxyGenerator.CreateClassProxyWithTarget(
                typeof(Juego),
                target,
                _interceptorGR) as INuevoRecord;
        }
    }
}