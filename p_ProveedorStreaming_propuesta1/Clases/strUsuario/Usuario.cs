using p_ProveedorStreaming.Eventos;
using p_ProveedorStreaming.Interfaces;
using System;
using static p_ProveedorStreaming.Clases.strUsuario.ReglasNegocioUsuario;

namespace p_ProveedorStreaming.Clases.strUsuario
{
    public class Usuario : ICambioCategoria
    {
        private static Random id_random = new();
        private int id_interno;
        private string nombre;
        private DateTime fecha_afiliacion;
        private ulong puntos;
        private l_categorias categoria;

        private PublisherCambioCategoria pub_cambio_cat = new();

        public Usuario(string nombre)
        {
            this.id_interno = id_random.Next(ReglasNegocioUsuario.min_id_usuario, ReglasNegocioUsuario.max_id_usuario);
            this.nombre = nombre;
            this.fecha_afiliacion = DateTime.Now;
            this.puntos = 0;
            this.categoria = l_categorias.General;

            this.pub_cambio_cat.EventoCambioCategoria += EventHandler;
        }

        // ACCESORES CORTOS
        public string Nombre => nombre;
        public ulong Puntos => puntos;
        public int Id_interno => id_interno;
        public DateTime Fecha_afiliacion => fecha_afiliacion;
        public l_categorias Categoria { get => categoria; set => categoria = value; }

        // MÉTODOS
        public void SumarPuntos(int cantidad)
        {
            if (cantidad > 0) this.puntos += (ulong)cantidad;
        }

        public int ObtenerPuntos() => (int)this.puntos;

        // IMPLEMENTACIÓN DE LA INTERFAZ
        // Este método realiza la lógica de cambio básica de la clase
        public void CambiarCategoria(Usuario usuario)
        {
            Console.WriteLine($"[Sistema] Verificando puntos de {usuario.Nombre}...");
            // Aquí podrías poner una lógica simple si no quieres usar la inyección
            if (usuario.Puntos > 1000) usuario.Categoria = l_categorias.Pro;
        }

        // SOBRECARGA PARA INYECCIÓN DE DEPENDENCIAS
        public void CambiarCategoria(Usuario usuario, ICambioCategoria logica)
        {
            logica.CambiarCategoria(usuario);
            pub_cambio_cat.InformarCambioCategoria(this, this.categoria);
        }

        public void EventHandler(Usuario usuario)
        {
            Console.WriteLine($"[LOG]: Usuario {usuario.Nombre} actualizado a {usuario.Categoria}");
        }
    }
}