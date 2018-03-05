using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketConversacion
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTicket { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public DateTime FechaGeneracion { get; set; }
        [DataMember]
        public bool Sistema { get; set; }
        [DataMember]
        public bool? Leido { get; set; }
        [DataMember]
        public bool? Privado { get; set; }
        [DataMember]
        public DateTime? FechaLectura { get; set; }
        [DataMember]
        public virtual Ticket Ticket { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual List<ConversacionArchivo> ConversacionArchivo { get; set; }
        [DataMember]
        public virtual List<TicketEventoConversacion> TicketEventoConversacion { get; set; }
    }
}
