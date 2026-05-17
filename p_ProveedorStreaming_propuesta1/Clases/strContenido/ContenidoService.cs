using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace p_ProveedorStreaming.Clases.strContenido
{
    public class ContenidoService
    {
        private List<Contenido> _contenidos;
        // Referencia a la fábrica que crea los proxies con aspectos
        private MensajeFactory _factory;

        public ContenidoService(MensajeFactory factory)
        {
            this._contenidos = new List<Contenido>();
            this._factory = factory;
        }

        // +Agregar(contenido: Contenido) : void
        public void Agregar(Contenido contenido)
        {
            if (contenido != null)
            {
                // IMPORTANTE: Aquí es donde se aplica el aspecto.
                // La factory envuelve el contenido original en un Proxy que tiene el interceptor.
                Contenido contenidoConAspecto = (Contenido)_factory.CrearMensajeGestionP(contenido);

                _contenidos.Add(contenidoConAspecto);
                Console.WriteLine($"[ContenidoService] '{contenido.Nombre}' agregado con gestión de puntos activa.");
            }
        }

        // +ObtenerTodos() : Contenido []
        public List<Contenido> ObtenerTodos()
        {
            return _contenidos;
        }

        // +ObtenerPorTitulo(nombre: string) : Contenido
        public Contenido ObtenerPorTitulo(string nombre)
        {
            var contenido = _contenidos.FirstOrDefault(c => c.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));

            if (contenido == null)
            {
                throw new Exception($"[Error] El contenido '{nombre}' no existe en el catálogo.");
            }

            return contenido;
        }
    }
}