using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketAsignacion
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTicket { get; set; }
        [DataMember]
        public int IdEstatusAsignacion { get; set; }
        [DataMember]
        public int? IdUsuarioAsigno { get; set; }
        [DataMember]
        public int? IdUsuarioAsignado { get; set; }
        [DataMember]
        public DateTime FechaAsignacion { get; set; }
        [DataMember]
        public string Comentarios { get; set; }
        [DataMember]
        public bool Visto { get; set; }

        [DataMember]
        public virtual Ticket Ticket { get; set; }
        [DataMember]
        public virtual EstatusAsignacion EstatusAsignacion { get; set; }
        [DataMember]
        public virtual Usuario UsuarioAsigno { get; set; }
        [DataMember]
        public virtual Usuario UsuarioAsignado { get; set; }
        [DataMember]
        public virtual List<TicketEventoAsignacion> TicketEventoAsignacion { get; set; }
    }
}
