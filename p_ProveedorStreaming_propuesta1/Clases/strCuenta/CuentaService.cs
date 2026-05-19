using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using System;
using System.Collections.Generic;
using System.IO;
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
        public List<Cuenta> ObtenerTodas() => _cuentas;

        public void Cargar(string nomArchivo, UsuarioService usuarioService)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", nomArchivo);

            if (!File.Exists(ruta))
                throw new FileNotFoundException($"No se encontró el archivo: {ruta}");

            foreach (var linea in File.ReadAllLines(ruta))
            {
                if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("#"))
                    continue;

                try
                {
                    var usuario = usuarioService.BuscarPorNombre(linea.Trim());
                    if (_cuentas.All(c => c.Usuario.Nombre != usuario.Nombre))
                        Crear(usuario);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error procesando línea '{linea}': {ex.Message}");
                }
            }
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