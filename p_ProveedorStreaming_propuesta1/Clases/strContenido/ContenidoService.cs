using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strContenido.Clases.strPelicula;
using p_ProveedorStreaming.Clases.strContenido.Clases.strSerie;
using p_ProveedorStreaming.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
                throw new Exception($"[Error] El contenido '{nombre}' no existe en el catálogo.");

            return contenido;
        }

        public void Cargar(string nomArchivo)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos", nomArchivo);

            if (!File.Exists(ruta))
                throw new FileNotFoundException($"No se encontró el archivo: {ruta}");

            foreach (var linea in File.ReadAllLines(ruta))
            {
                if (string.IsNullOrWhiteSpace(linea) || linea.StartsWith("#"))
                    continue;

                var datos = linea.Split('|');

                try
                {
                    string tipo = datos[0].Trim().ToUpper();

                    if (tipo == "P")
                    {
                        // P|nombre|duracion(hh:mm:ss)|calificacion
                        if (datos.Length != 4)
                            throw new Exception("Formato película: P|nombre|hh:mm:ss|calificacion");

                        Agregar(new Pelicula(
                            datos[1].Trim(),
                            TimeSpan.Parse(datos[2].Trim()),
                            byte.Parse(datos[3].Trim())
                        ));
                    }
                    else if (tipo == "S")
                    {
                        // S|nombre|temporadas|cap_x_temp
                        if (datos.Length != 4)
                            throw new Exception("Formato serie: S|nombre|temporadas|cap_x_temp");

                        Agregar(new Serie(
                            datos[1].Trim(),
                            byte.Parse(datos[2].Trim()),
                            byte.Parse(datos[3].Trim())
                        ));
                    }
                    else
                    {
                        throw new Exception($"Tipo desconocido '{tipo}'. Use P (película) o S (serie).");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error procesando línea '{linea}': {ex.Message}");
                }
            }
        }
    }
}