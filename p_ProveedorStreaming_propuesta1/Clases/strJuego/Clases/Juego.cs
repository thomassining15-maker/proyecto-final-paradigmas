using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Interfaces;
using p_ProveedorStreaming.Eventos;
using System;

namespace p_ProveedorStreaming.Clases.strJuego.Clases
{
    public class Juego : INuevoRecord
    {
        // Atributos según el diagrama
        private string nombre;
        private string genero;
        private ulong nro_record;
        private Usuario? usuario_record;

        // Publishers y eventos
        public PublisherNuevoTitulo pub_nuevo_tit = new();
        public PublisherJuego pub_nuevo_rec = new();

        // Requerido por Castle.DynamicProxy para generar el proxy de clase
        protected Juego() { nombre = string.Empty; genero = string.Empty; }

        public Juego(string nombre, string genero)
        {
            this.nombre = nombre;
            this.genero = genero;
            this.nro_record = 0;
            this.usuario_record = null;
        }

        // ACCESORES CORTOS
        public virtual string Nombre => nombre;
        public virtual string Genero => genero;
        public virtual ulong Nro_record { get => nro_record; set => nro_record = value; }
        public virtual Usuario? Usuario_record { get => usuario_record; set => usuario_record = value; }

        // MÉTODOS DE NOTIFICACIÓN
        public void ObtenerNuevoTitulo(object titulo, INuevoTitulo notificador)
        {
            pub_nuevo_tit.InformarNuevoTitulo(titulo);
        }

        public void EventHandler(object titulo)
        {
            Console.WriteLine($"[Juego] Notificación de nuevo título: {titulo}");
        }

        // --- ASPECTOS: MÉTODO INTERCEPTABLE ---
        // Lo dejamos VIRTUAL para que el Proxy pueda envolverlo.
        // Aquí NO va la lógica de validación, eso lo hace el Interceptor_GestionRecord.
        public virtual void ObtenerNuevoRecord(ulong nro_record, Usuario usuario_record, Juego juego, INuevoRecord validador)
        {
            // Este método se ejecuta DESPUÉS de que el interceptor valida el récord.
            this.nro_record = nro_record;
            this.usuario_record = usuario_record;

            // Disparamos el evento de nuevo récord
            pub_nuevo_rec.InformarNuevoRecord(nro_record);
        }

        public void EventHandler(ulong nro_record)
        {
            Console.WriteLine($"[RÉCORD] ¡Nuevo récord de {nro_record} alcanzado en {nombre}!");
        }

        // Virtual para que Castle delegue al target donde nombre tiene el valor real
        public virtual string RegistrarPuntaje(Usuario usuario, byte puesto)
        {
            int puntos;
            switch (puesto)
            {
                case 1: puntos = ReglasNegocioJuego.ptos_primerpuesto; break;
                case 2: puntos = ReglasNegocioJuego.ptos_segundopuesto; break;
                case 3: puntos = ReglasNegocioJuego.ptos_tercerpuesto; break;
                default: puntos = ReglasNegocioJuego.ptos_resto; break;
            }

            usuario.SumarPuntos(puntos);
            usuario.CambiarCategoria(usuario);
            return $"[Juego] +{puntos} pts a {usuario.Nombre} (puesto {puesto}) en '{nombre}'";
        }
    }
}