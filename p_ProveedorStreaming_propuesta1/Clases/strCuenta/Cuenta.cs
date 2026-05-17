using p_ProveedorStreaming.Clases.strContenido.Clases;
using p_ProveedorStreaming.Clases.strUsuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Clases.strCuenta
{
    public class Cuenta
    {
        private Usuario usuario;
        private List<Contenido> l_contenidovisto;

        public Cuenta(Usuario usuario)
        {
            this.usuario = usuario;
            // Inicializamos la lista en el constructor para que el método Agregar funcione
            this.l_contenidovisto = new List<Contenido>();
        }
        public Usuario Usuario => usuario;
        public void AgregarContenidoVisto(Contenido contenido)
        {
            // Verificamos que el contenido exista antes de añadirlo a la lista
            if (contenido != null)
            {
                l_contenidovisto.Add(contenido);
            }
        }
    }
}