using Castle.DynamicProxy;
using System;

namespace p_ProveedorStreaming.Aspectos
{
    public class Interceptor_Validacion : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "Registrar")
            {
                object entidad = invocation.Arguments[0];

                // PRE: validar que el objeto no sea nulo antes de "guardar"
                if (entidad == null)
                    throw new Exception("[Validación] No se puede registrar un objeto nulo en la BD.");

                Console.WriteLine($"[Aspecto Validación] Validando datos de: {entidad}...");

                invocation.Proceed();

                // POST: simular confirmación de guardado en BD
                Console.WriteLine($"[Aspecto Validación] Guardado simulado en BD: OK — {entidad}");
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
