using Castle.DynamicProxy;
using p_ProveedorStreaming.Interfaces;
using System;

namespace p_ProveedorStreaming.Aspectos
{
    public class Interceptor_GestionPuntos : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            // Intercepta el método de reproducción para disparar la lógica de puntos
            if (invocation.Method.Name == "Reproducir")
            {
                var contenido = invocation.InvocationTarget as IActualizacionPuntos;
                contenido?.ActualizarPuntaje();
            }
        }
    }
}