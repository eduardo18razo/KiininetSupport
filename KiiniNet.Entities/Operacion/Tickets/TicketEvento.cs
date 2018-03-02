using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketEvento
    {
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public int IdTicket { get; set; }
        [DataMember]
        public int IdUsuarioRealizo { get; set; }
        [DataMember]
        public DateTime FechaHora { get; set; }
        [DataMember]
        public virtual Ticket Ticket { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual List<TicketEventoAsignacion> TicketEventoAsignacion { get; set; }
        [DataMember]
        public virtual List<TicketEventoConversacion> TicketEventoConversacion { get; set; }
        [DataMember]
        public virtual List<TicketEventoEstatus> TicketEventoEstatus { get; set; }
    }
}
