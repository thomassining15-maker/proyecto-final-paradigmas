using Castle.DynamicProxy;
using System;

namespace p_ProveedorStreaming.Aspectos
{
    public class Interceptor_Autenticacion : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "IngresarSistema")
            {
                string usuario = invocation.Arguments[0]?.ToString() ?? "";
                string clave   = invocation.Arguments[1]?.ToString() ?? "";

                // PRE: registrar intento de autenticación
                Console.WriteLine($"[Aspecto Autenticación] Intento de acceso — usuario: '{usuario}'");

                if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
                    throw new Exception("[Autenticación] Usuario o clave vacíos.");

                invocation.Proceed();

                // POST: confirmar resultado
                bool resultado = invocation.ReturnValue is bool b && b;
                Console.WriteLine($"[Aspecto Autenticación] Acceso {(resultado ? "CONCEDIDO" : "DENEGADO")} para '{usuario}'");
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
