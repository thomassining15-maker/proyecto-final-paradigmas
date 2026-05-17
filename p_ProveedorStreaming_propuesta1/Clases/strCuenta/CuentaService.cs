using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace p_ProveedorStreaming.Clases.strCuenta
{
    public class CuentaService
    {
        // -_cuentas: Cuenta []
        private List<Cuenta> _cuentas = new();
        
        public void Crear(Usuario usuario)
        {
            if (usuario != null)
            {
                Cuenta nuevaCuenta = new Cuenta(usuario);
                _cuentas.Add(nuevaCuenta);
                Console.WriteLine($"[CuentaService] Cuenta creada para: {usuario.Nombre}");
            }
        }

        public Cuenta ObtenerCuentaPorUsuario(ulong idUsuario)
        {
            return _cuentas.FirstOrDefault(c => (ulong)c.Usuario.Id_interno == idUsuario);
        }
        public void RegistrarVisualizacion(ulong idCuenta, Contenido contenido)
        {
            // Buscamos la cuenta por el ID del usuario vinculado
            Cuenta cuenta = _cuentas.FirstOrDefault(c => (ulong)c.Usuario.Id_interno == idCuenta);

            if (cuenta != null && contenido != null)
            {
                cuenta.AgregarContenidoVisto(contenido);
                Console.WriteLine($"[Visualización] '{contenido.Nombre}' registrado en la cuenta de {cuenta.Usuario.Nombre}");
            }
            else
            {
                Console.WriteLine("[Error] ID de cuenta no encontrado o contenido nulo.");
            }
        }
    }
}