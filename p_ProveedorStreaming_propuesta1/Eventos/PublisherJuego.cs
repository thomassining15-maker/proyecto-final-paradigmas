using System;
using System.Collections.Generic;
using System.Text;

namespace p_ProveedorStreaming.Eventos
{
    public class PublisherJuego
    {
        // Firma: +dele_nuevo_record (ulong record) : void delegate
        public delegate void dele_nuevo_record(ulong record);

        // Evento: +dele_nuevo_record : event dele_nuevo_record
        public event dele_nuevo_record EventoNuevoRecord;

        // Método: +InformarNuevoRecord (record:ulong) : string
        public string InformarNuevoRecord(ulong record)
        {
            EventoNuevoRecord?.Invoke(record);
            return $"Nuevo récord de {record} notificado.";
        }
    }
}