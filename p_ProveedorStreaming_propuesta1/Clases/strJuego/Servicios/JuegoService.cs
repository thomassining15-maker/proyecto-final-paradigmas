using p_ProveedorStreaming.Clases.strJuego.Clases;
using p_ProveedorStreaming.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace p_ProveedorStreaming.Clases.strJuego
{
    public class JuegoService
    {
        private List<Juego> _juegos;
        private MensajeFactory _factory;

        public JuegoService(MensajeFactory factory)
        {
            this._juegos = new List<Juego>();
            this._factory = factory;
        }

        // +Agregar(juego: Juego) : void
        public void Agregar(Juego juego)
        {
            if (juego != null)
            {
                // Se crea el proxy usando la factory para activar el Interceptor_GestionRecord
                Juego juegoConAspecto = (Juego)_factory.CrearMensajeGestionR(juego);
                _juegos.Add(juegoConAspecto);
                Console.WriteLine($"[JuegoService] Juego '{juego.Nombre}' registrado con soporte de aspectos.");
            }
        }

        public List<Juego> ObtenerTodos()
        {
            return _juegos;
        }

        public Juego BuscarPorNombre(string nombre)
        {
            var juego = _juegos.FirstOrDefault(j => j.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (juego == null) throw new Exception($"No se encontró el juego: {nombre}");
            return juego;
        }

        public Juego BuscarPorGenero(string genero)
        {
            var juego = _juegos.FirstOrDefault(j => j.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));
            if (juego == null) throw new Exception($"No hay juegos del género: {genero}");
            return juego;
        }

        public ulong ObtenerRecord(string nombre)
        {
            return BuscarPorNombre(nombre).Nro_record;
        }

        public p_ProveedorStreaming.Clases.strUsuario.Usuario ObtenerMejorJugador(string nombre)
        {
            return BuscarPorNombre(nombre).Usuario_record;
        }

        public void IniciarPartida()
        {
            Console.WriteLine("[Sistema] Iniciando sesión de juego...");
        }

        public void FinalizarPartida()
        {
            Console.WriteLine("[Sistema] Partida finalizada. Verificando récords vía interceptor...");
        }
    }
}