using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Clases.strUsuario
{
    public static class ReglasNegocioUsuario
    {
        public enum l_categorias {Master, Pro, General};
        public static readonly int puntos_general = 700;
        public static readonly int puntos_pro = 2000;
        public static readonly int puntos_master = 4000;
        public static readonly int min_id_usuario = 1;
        public static readonly int max_id_usuario= 9999;
    }
}
