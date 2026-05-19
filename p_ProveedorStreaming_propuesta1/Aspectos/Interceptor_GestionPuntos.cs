using Castle.DynamicProxy;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Interfaces;

namespace p_ProveedorStreaming.Aspectos
{
    public class Interceptor_GestionPuntos : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (invocation.Method.Name == "Reproducir")
            {
                // El proxy y el target son objetos distintos en Castle.DynamicProxy.
                // UsuarioActivo se asigna sobre el proxy, así que debemos sincronizarlo
                // al target antes de llamar ActualizarPuntaje(), que corre en el target.
                if (invocation.Proxy is Contenido proxyContenido &&
                    invocation.InvocationTarget is Contenido targetContenido)
                {
                    targetContenido.UsuarioActivo = proxyContenido.UsuarioActivo;
                }

                var contenido = invocation.InvocationTarget as IActualizacionPuntos;
                contenido?.ActualizarPuntaje();
            }
        }
    }
}