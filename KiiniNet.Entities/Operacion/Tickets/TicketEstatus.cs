using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketEstatus
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdTicket { get; set; }

        [DataMember]
        public int IdEstatus { get; set; }

        [DataMember]
        public DateTime FechaMovimiento { get; set; }
        [DataMember]
        public String Comentarios { get; set; }

        [DataMember]
        public int IdUsuarioMovimiento { get; set; }

        [DataMember]
        public virtual Ticket Ticket { get; set; }

        [DataMember]
        public virtual EstatusTicket EstatusTicket { get; set; }

        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual List<TicketEventoEstatus> TicketEventoEstatus { get; set; }
    }
}