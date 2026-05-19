using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Clases.strJuego.Clases;
using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strCuenta;
using System;
using System.Collections.Generic;

namespace p_ProveedorStreaming.Clases.strProveedor
{
    public class ProveedorService
    {
        private Proveedor proveedor;

        public ProveedorService(Proveedor proveedor)
        {
            this.proveedor = proveedor;
        }

        // +RegistrarCuenta(nuevo_usuario: Usuario) : void
        public void RegistrarCuenta(Usuario nuevo_usuario)
        {
            if (nuevo_usuario != null)
            {
                // Se crea la cuenta vinculada al usuario
                Cuenta nuevaCuenta = new Cuenta(nuevo_usuario);

                // Se agrega a la lista global del proveedor
                proveedor.L_cuentas.Add(nuevaCuenta);

                Console.WriteLine($"[Registro] Cuenta creada para el usuario: {nuevo_usuario.Nombre}");
            }
        }

        // +BuscarCuentas() : List<Cuenta>
        public List<Cuenta> BuscarCuentas()
        {
            Console.WriteLine("[Consulta] Obteniendo lista de todas las cuentas registradas...");
            return proveedor.L_cuentas;
        }

        // +BuscarContenidos() : List<Contenido>
        public List<Contenido> BuscarContenidos()
        {
            Console.WriteLine("[Catálogo] Cargando lista completa de contenidos (Películas/Series)...");
            return proveedor.L_contenidos;
        }

        // +BuscarJuegos() : List<Juego>
        public List<Juego> BuscarJuegos()
        {
            Console.WriteLine("[Catálogo] Cargando lista completa de juegos disponibles...");
            return proveedor.L_juegos;
        }
    }
}