using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strCuenta;
using p_ProveedorStreaming.Clases.strJuego.Clases;
using System;
using System.Collections.Generic;

namespace p_ProveedorStreaming.Clases.strProveedor
{
    public class Proveedor
    {
        private string nombre;
        private List<Cuenta> l_cuentas;
        private List<Contenido> l_contenidos;
        private List<Juego> l_juegos;

        public Proveedor(string nombre)
        {
            this.nombre = nombre;
            this.l_cuentas = new List<Cuenta>();
            this.l_contenidos = new List<Contenido>();
            this.l_juegos = new List<Juego>();
        }

        public string Nombre => nombre;

        public List<Cuenta> L_cuentas
        {
            get => l_cuentas;
            set => l_cuentas = value;
        }

        public List<Contenido> L_contenidos
        {
            get => l_contenidos;
            set => l_contenidos = value;
        }

        public List<Juego> L_juegos
        {
            get => l_juegos;
            set => l_juegos = value;
        }
    }
}