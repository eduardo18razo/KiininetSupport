using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketEventoAsignacion
    {
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public Int64 IdTicketEvento { get; set; }
        [DataMember]
        public int IdTicketAsignacion { get; set; }
        [DataMember]
        public DateTime FechaHora { get; set; }
        [DataMember]
        public virtual TicketEvento TicketEvento { get; set; }
        [DataMember]
        public virtual TicketAsignacion TicketAsignacion { get; set; }
    }
}
