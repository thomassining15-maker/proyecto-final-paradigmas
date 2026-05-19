using Castle.DynamicProxy;
using p_ProveedorStreaming.Clases.strJuego.Clases;
using System;

namespace p_ProveedorStreaming.Aspectos
{
    public class Interceptor_GestionRecord : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "ObtenerNuevoRecord")
            {
                // PRE: validar el nuevo record antes de registrarlo
                ulong nuevoRecord = invocation.Arguments[0] is ulong r ? r : 0;
                ulong recordActual = (invocation.InvocationTarget as Juego)?.Nro_record ?? 0;

                if (nuevoRecord <= recordActual)
                {
                    Console.WriteLine($"[Aspecto Récord] {nuevoRecord} no supera el récord actual ({recordActual}). Operación cancelada.");
                    return;
                }

                Console.WriteLine($"[Aspecto Récord] Validando: {nuevoRecord} > {recordActual}. Récord válido, procediendo...");
                invocation.Proceed();

                // POST: sincronizar el estado del target de vuelta al proxy
                if (invocation.InvocationTarget is Juego target && invocation.Proxy is Juego proxy)
                {
                    proxy.Nro_record     = target.Nro_record;
                    proxy.Usuario_record = target.Usuario_record;
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}