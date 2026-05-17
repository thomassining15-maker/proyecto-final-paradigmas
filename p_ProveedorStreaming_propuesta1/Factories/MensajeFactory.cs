using Castle.DynamicProxy;
using p_ProveedorStreaming.Interfaces;
using p_ProveedorStreaming.Clases;
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
        private readonly Interceptor_Validacion _interceptorVal;
        private readonly Interceptor_Autenticacion _interceptorAuth;

        public MensajeFactory(
            Interceptor_GestionPuntos interceptorPuntos,
            Interceptor_GestionRecord interceptorRecord,
            Interceptor_Validacion interceptorValidacion,
            Interceptor_Autenticacion interceptorAutenticacion)
        {
            _proxyGenerator   = new ProxyGenerator();
            _interceptorGP    = interceptorPuntos;
            _interceptorGR    = interceptorRecord;
            _interceptorVal   = interceptorValidacion;
            _interceptorAuth  = interceptorAutenticacion;
        }

        // AOP: gestión de puntos al reproducir contenido
        public IActualizacionPuntos CrearMensajeGestionP(object target)
        {
            return _proxyGenerator.CreateClassProxyWithTarget(
                target.GetType(),
                target,
                _interceptorGP) as IActualizacionPuntos;
        }

        // AOP: validación de récord antes de registrarlo
        public INuevoRecord CrearMensajeGestionR(Juego target)
        {
            return _proxyGenerator.CreateClassProxyWithTarget(
                typeof(Juego),
                target,
                _interceptorGR) as INuevoRecord;
        }

        // AOP: simula validación y guardado en BD
        public IRegistroEntidad CrearMensajeValidacion(RegistroBD target)
        {
            return _proxyGenerator.CreateInterfaceProxyWithTarget<IRegistroEntidad>(
                target,
                _interceptorVal);
        }

        // AOP: autentica al usuario antes de permitir acceso al sistema
        public IAcceso CrearMensajeAutenticacion(AccesoSistema target)
        {
            return _proxyGenerator.CreateInterfaceProxyWithTarget<IAcceso>(
                target,
                _interceptorAuth);
        }
    }
}