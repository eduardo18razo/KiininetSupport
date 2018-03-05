using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketEventoConversacion
    {
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public Int64 IdTicketEvento { get; set; }
        [DataMember]
        public int IdTicketConversacion { get; set; }
        [DataMember]
        public DateTime FechaHora { get; set; }
        [DataMember]
        public virtual TicketEvento TicketEvento { get; set; }
        [DataMember]
        public virtual TicketConversacion TicketConversacion { get; set; }
    }
}
