using System;
using System.Collections.Generic;
using System.Text;
using p_ProveedorStreaming.Clases.strUsuario;
using p_ProveedorStreaming.Clases.strJuego.Clases;

namespace p_ProveedorStreaming.Interfaces
{
    public interface INuevoRecord
    {
        void ObtenerNuevoRecord(ulong nro_record, Usuario usuario_record, Juego juego, INuevoRecord validador);
    }
}