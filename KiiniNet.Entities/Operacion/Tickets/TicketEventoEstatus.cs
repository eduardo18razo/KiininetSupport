using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketEventoEstatus
    {
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public Int64 IdTicketEvento { get; set; }
        [DataMember]
        public int IdTicketEstatus { get; set; }
        [DataMember]
        public DateTime FechaHora { get; set; }
        [DataMember]
        public virtual TicketEvento TicketEvento { get; set; }
        [DataMember]
        public virtual TicketEstatus TicketEstatus { get; set; }
    }
}
