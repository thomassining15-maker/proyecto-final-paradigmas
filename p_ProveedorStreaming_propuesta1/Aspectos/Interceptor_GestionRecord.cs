using Castle.DynamicProxy;
using System;

namespace p_ProveedorStreaming.Aspectos
{
    public class Interceptor_GestionRecord : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "ObtenerNuevoRecord")
            {
                // Aquí se ejecuta la lógica de validación de récord antes de proceder
                Console.WriteLine("[Aspecto] Validando récord en el interceptor...");
            }

            invocation.Proceed();
        }
    }
}